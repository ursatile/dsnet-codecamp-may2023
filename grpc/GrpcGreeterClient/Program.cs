using Grpc.Net.Client;
using GrpcGreeter;

using var channel = GrpcChannel.ForAddress("http://localhost:5235");
var grpcClient = new Greeter.GreeterClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
while (true) {
	Console.ReadKey(true);
	var request = new HelloRequest { Name = "CodeCamp" };
	var reply = await grpcClient.SayHelloAsync(request);
	Console.WriteLine("Greeting: " + reply.Message);
}

