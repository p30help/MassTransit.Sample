using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Models;

namespace MassTransit.Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Application started");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("order-queue", e =>
                {
                    e.Consumer<OrderConsumer>();
                });

                cfg.ReceiveEndpoint("user-queue", e =>
                {
                    e.PrefetchCount = 10;

                    e.Consumer<UserConsumer>();
                });

            });

            await busControl.StartAsync();
            
            try
            {
                Console.WriteLine("Press enter to exit or wait ro receive");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }

            //var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            //{
            //    config.Host("amqp://guest:guest@localhost:5672");
            //
            //    config.ReceiveEndpoint("order-queue", c =>
            //    {
            //        c.Handler<Order>(ctx =>
            //        {
            //            return Console.Out.WriteLineAsync(ctx.Message.Name);
            //        });
            //    });
            //
            //});
            //
            //bus.Start();
            
        }

        class OrderConsumer : IConsumer<Order>
        {
            public async Task Consume(ConsumeContext<Order> context)
            {
                Console.WriteLine("Order Value: {0}", context.Message.Name);
            }
        }

        class UserConsumer : IConsumer<User>
        {
            public async Task Consume(ConsumeContext<User> context)
            {
                Console.WriteLine("User Name: {0}", context.Message.Username);
            }
        }
    }
}
