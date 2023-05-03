using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services {

	public class GreeterService : Greeter.GreeterBase {
		private readonly ILogger<GreeterService> _logger;
		public GreeterService(ILogger<GreeterService> logger) {
			_logger = logger;
		}

		public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            _logger.LogInformation("Request {request}", request.Number);
            var name = $"{request.FirstName} {request.LastName}";
			var message = request.LanguageCode switch {
				"en-GB" => $"Good morning, {name}",
				"en-US" => $"Howdy, {name}",
				"ro-RO" => $"Buna dimineata, {name}!",
				"ru-RU" => $"Доброе Утро, {name}",
				"fr-FR" => $"Bonjour, {name}",
				_ => $"Hello, {name}"
			};
			return Task.FromResult(new HelloReply {
				Message = message
			});
		}
	}
}
namespace GrpcGreeter {
	public partial class HelloRequest {
		public string Name => FirstName + " " + LastName;
	}
}