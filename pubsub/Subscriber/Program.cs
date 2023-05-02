using EasyNetQ;
using Messages;

const string AMQP = "amqps://qahpngku:S1kHZJTwNBOtdq8cRLvqpiUMClaMVFrf@heavy-white-jackal.rmq5.cloudamqp.com/qahpngku";
var bus = RabbitHutch.CreateBus(AMQP);

var subscriptionId = "dylan-subscriber-2";

bus.PubSub.Subscribe<Greeting>(subscriptionId, Handle);

Console.WriteLine("Listening for messages. Press Enter to exit...");
Console.ReadLine();

void Handle(Greeting g) {
    if (g.Message.Contains("5")) {
        throw new Exception("5 is not allowed!");
    }
    Console.WriteLine(g);
}

