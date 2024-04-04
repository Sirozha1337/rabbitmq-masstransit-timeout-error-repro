using MassTransit;
using Messages;

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(options =>
{
    options.Host(new Uri($"amqps://guest:guest@localhost:5672/"));
});

bus.Start();

string input;
Console.WriteLine("Enter a message. 'Quit' to quit.");
while ((input = Console.ReadLine()) != "Quit") 
{
    try
    {
        var pub1 = bus.Publish(new TextMessage { Text = $"From Publisher1: {input}" });
        var pub2 = bus.Publish(new TextMessage { Text = $"From Publisher2: {input}" });

        try
        {
            await pub2;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error! {e.Message}");
        }
        
        try
        {
            await pub1;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error! {e.Message}");
        }
        
        Console.WriteLine("Message published!");
    }
    catch(Exception e)
    {
        Console.WriteLine($"Error! {e.Message}");
    }
}
bus.Stop();

