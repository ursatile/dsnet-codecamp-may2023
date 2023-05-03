using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autobarn.Pricing;
using Grpc.Net.Client;

var builder = Host.CreateDefaultBuilder()
	.ConfigureLogging((hostingContext, logging) => {
		logging.ClearProviders();
		logging.AddConsole();
	})
	.ConfigureServices((context, services) => {
		var amqp = context.Configuration.GetConnectionString("amqp");
		var bus = RabbitHutch.CreateBus(amqp);
		var channel = GrpcChannel.ForAddress("https://localhost:7152");
		var grpcClient = new Pricer.PricerClient(channel);
		services.AddSingleton(grpcClient);
		services.AddSingleton(bus);
		services.AddHostedService<PricingClientService>();
	});

var host = builder.Build();
host.Run();

public class PricingClientService : IHostedService {
	private readonly ILogger<PricingClientService> logger;
	private readonly IBus bus;
	private readonly Pricer.PricerClient grpcClient;
	private string subscriptionId = $"autobarn.auditlog@{Environment.MachineName}";

	public PricingClientService(ILogger<PricingClientService> logger,
	 IBus bus,
	 Pricer.PricerClient grpcClient
	 ) {
		this.logger = logger;
		this.bus = bus;
		this.grpcClient = grpcClient;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Started AuditLogService");
		logger.LogCritical("This is a critical message!");
		logger.LogWarning("This is a warning!");
		logger.LogError("This one is an error!");
		logger.LogDebug("This is a DEBUG message");
		logger.LogTrace("This one is a trace");
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
		logger.LogDebug("Received NewVehicleListed message: {message}", message);
		var request = new PriceRequest {
			Make = message.Make,
			Model = message.Model,
			Year = message.Year
		};
		var reply = await grpcClient.GetPriceAsync(request);
		logger.LogInformation("Price for {make} {model} {year} is {price} {currency}",
			message.Make, message.Model, message.Year, reply.Price, reply.CurrencyCode);
	}
}
