using EasyNetQ;
using Messages;

const string AMQP = "amqps://qahpngku:S1kHZJTwNBOtdq8cRLvqpiUMClaMVFrf@heavy-white-jackal.rmq5.cloudamqp.com/qahpngku";
var bus = RabbitHutch.CreateBus(AMQP);

Console.WriteLine("Press any key to publish...");
int counter = 0;
while(true) {
    Console.ReadKey(true);
    var greeting = new Greeting($"Hello from CodeCamp #{counter++}");
    bus.PubSub.Publish(greeting);
    Console.WriteLine($"Published: {greeting}");
}

