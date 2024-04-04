using MassTransit;
using Messages;

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(options =>
{
    options.Host(new Uri($"amqps://guest:guest@localhost:5672/"));
    options.ReceiveEndpoint("MyQueue", e =>
    {
        e.UseDelayedRedelivery(r => r.Immediate(2));
        e.Consumer<MyTextMessageConsumer>();
    });
});
bus.Start();
Console.WriteLine("Listening for messages. Hit <return> to quit.");
Console.ReadLine();
bus.Stop();

class MyTextMessageConsumer : IConsumer<TextMessage>
{
    public Task Consume(ConsumeContext<TextMessage> context)
    {
        var textMessage = context.Message;
        Console.WriteLine("Got message {0} {1}", context.MessageId, textMessage.Text);

        return Task.CompletedTask;
    }
}