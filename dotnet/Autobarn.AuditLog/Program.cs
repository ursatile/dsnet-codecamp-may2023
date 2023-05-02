using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder()
	.ConfigureLogging((hostingContext, logging) => {
		logging.ClearProviders();
		logging.AddConsole();
	})
	.ConfigureServices((context, services) => {
		var amqp = context.Configuration.GetConnectionString("amqp");
		var bus = RabbitHutch.CreateBus(amqp);
		services.AddSingleton(bus);
		services.AddHostedService<AuditLogService>();
	});

var host = builder.Build();
host.Run();

public class AuditLogService : IHostedService {
	private readonly ILogger<AuditLogService> logger;
	private readonly IBus bus;

	private string subscriptionId = $"autobarn.auditlog@{Environment.MachineName}";

	public AuditLogService(ILogger<AuditLogService> logger, IBus bus) {
		this.logger = logger;
		this.bus = bus;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Started AuditLogService");
		return bus.PubSub.SubscribeAsync<NewVehicleListed>(
			subscriptionId,
			HandleNewVehicleListed
		);
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopped AuditLogService");
		return Task.CompletedTask;
	}

	private async Task HandleNewVehicleListed(NewVehicleListed message) {
		logger.LogInformation($"START {Thread.CurrentThread.ManagedThreadId} NewVehicleListed: {message}");
		await Task.Delay(TimeSpan.FromSeconds(1));
		logger.LogInformation($"STOP {Thread.CurrentThread.ManagedThreadId} NewVehicleListed: {message}");
	}
}
