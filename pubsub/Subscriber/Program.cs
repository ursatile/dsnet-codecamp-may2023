using EasyNetQ;
using Messages;

const string AMQP = "amqps://qahpngku:S1kHZJTwNBOtdq8cRLvqpiUMClaMVFrf@heavy-white-jackal.rmq5.cloudamqp.com/qahpngku";
var bus = RabbitHutch.CreateBus(AMQP);

var subscriptionId = "dylan-subscriber";
bus.PubSub.Subscribe<Greeting>(subscriptionId,
    greeting => Console.WriteLine(greeting)
);
Console.WriteLine("Listening for messages. Press Enter to exit...");
Console.ReadLine();

