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

	public AuditLogService(ILogger<AuditLogService> logger) {
		this.logger = logger;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Started AuditLogService");
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopped AuditLogService");
		return Task.CompletedTask;
	}
}
