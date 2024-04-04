using MassTransit;
using MassTransit.Logging;
using Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(options =>
{
    options.Host(new Uri($"amqps://guest:guest@localhost:5672/"));
    
    LogContext.ConfigureCurrentLogContext(new TextWriterLoggerFactory(Console.Out, 
        new OptionsWrapper<TextWriterLoggerOptions>(new TextWriterLoggerOptions()
    {
        LogLevel = LogLevel.Debug
    })));
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
        var pub3 = bus.Publish(new TextMessage { Text = $"From Publisher3: {input}" });
        var pub4 = bus.Publish(new TextMessage { Text = $"From Publisher4: {input}" });

        try
        {
            await pub2;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Pub2 Error! {e.Message}");
        }
        
        try
        {
            await pub3;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Pub3 Error! {e.Message}");
        }
        
        try
        {
            await pub4;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Pub4 Error! {e.Message}");
        }
        
        try
        {
            await pub1;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Pub1 Error! {e.Message}");
        }
        
        Console.WriteLine("Message published!");
    }
    catch(Exception e)
    {
        Console.WriteLine($"Error! {e.Message}");
    }
}
bus.Stop();

