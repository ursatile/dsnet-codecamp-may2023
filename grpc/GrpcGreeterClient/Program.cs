using Grpc.Net.Client;
using GrpcGreeter;

using var channel = GrpcChannel.ForAddress("http://localhost:5235");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
// 			"en-GB" => $"Good morning, {request.Name}",
			// "en-US" => $"Howdy, {request.Name}",
			// "ro-RO" => $"Buna dimineata, {request.Name}!",
			// "ru-RU" => $"Доброе Утро, {request.Name}",
			// "fr-FR" => $"Bonjour, {request.Name}",
while (true) {
	var key = Console.ReadKey(true);
	var languageCode = key.KeyChar switch {
		'0' => "en-GB",
		'1' => "en-US",
		'2' => "ro-RO",
		'3' => "ru-RU",
		'4' => "fr-FR",
		_ => ""
	};
	var request = new HelloRequest { FirstName = "CodeCamp", LastName = "Bucharest", LanguageCode = languageCode };
	var reply = await grpcClient.SayHelloAsync(request);
	Console.WriteLine("Greeting: " + reply.Message);
}

