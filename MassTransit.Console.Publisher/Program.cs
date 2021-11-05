using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Models;

namespace MassTransit.Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                while (true)
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    //await busControl.Publish<Order>(new Order
                    //{
                    //    Name = value
                    //});

                    var userMsg = new User
                    {
                        Username = value
                    };
                    await busControl.Publish<User>(userMsg);

                    //var endpoint = await busControl.GetSendEndpoint(new Uri("queue:order-queue"));
                    //await endpoint.Send<User>(userMsg, context =>
                    //{
                    //});
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
