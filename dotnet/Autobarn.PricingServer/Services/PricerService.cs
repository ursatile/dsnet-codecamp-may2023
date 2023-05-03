using Grpc.Core;
using Autobarn.Pricing;

namespace Autobarn.PricingServer.Services;

public class PricerService : Pricer.PricerBase {
	private readonly ILogger<PricerService> _logger;
	public PricerService(ILogger<PricerService> logger) {
		_logger = logger;
	}

	public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
		return Task.FromResult(new PriceReply {
			CurrencyCode = "EUR",
			Price = 12345678
		});
	}
}
