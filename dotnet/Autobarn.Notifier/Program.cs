using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

var builder = Host.CreateDefaultBuilder()
	.ConfigureLogging((hostingContext, logging) => {
		logging.ClearProviders();
		logging.AddConsole();
	})
	.ConfigureServices((context, services) => {
		var amqp = context.Configuration.GetConnectionString("amqp");
		var bus = RabbitHutch.CreateBus(amqp);
		var hubUrl = context.Configuration["SignalRHubUrl"];
		var hub = new HubConnectionBuilder().WithUrl(hubUrl).Build();
		services.AddSingleton(hub);
		services.AddSingleton(bus);
		services.AddHostedService<PricingNotifierService>();
	});

var host = builder.Build();
host.Run();

public class PricingNotifierService : IHostedService {
	private readonly ILogger<PricingNotifierService> logger;
	private readonly IBus bus;
	private readonly HubConnection hub;

	private string subscriptionId = $"autobarn.pricing@{Environment.MachineName}";

	public PricingNotifierService(ILogger<PricingNotifierService> logger, IBus bus, HubConnection hub) {
		this.logger = logger;
		this.bus = bus;
		this.hub = hub;
	}

	public async Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Started PricingNotifierService");
		await hub.StartAsync(cancellationToken);
		logger.LogInformation("Connected to SignalR Hub!");
		await bus.PubSub.SubscribeAsync<NewVehiclePrice>(
			subscriptionId,
			HandleNewVehiclePrice
		);
	}

	public async Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping PricingNotifierService...");
		await hub.StopAsync(cancellationToken);
		logger.LogInformation("Stopped SignalR Hub");
	}

	private async Task HandleNewVehiclePrice(NewVehiclePrice message) {
		logger.LogInformation($"START {Thread.CurrentThread.ManagedThreadId} NewVehiclePrice: {message}");
		var json = JsonConvert.SerializeObject(message);
		await hub.SendAsync("NotifyUsersAsPartOfCodecampWorkshop", "autobarn.notifier", json);
		logger.LogInformation($"STOP {Thread.CurrentThread.ManagedThreadId} NewVehiclePrice: {message}");
	}
}
