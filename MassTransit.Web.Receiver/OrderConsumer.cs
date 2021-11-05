using RabbitMQ.Models;
using System;
using System.Threading.Tasks;

namespace MassTransit.Web.Receiver
{
    public class OrderConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            Console.WriteLine("Order Value: {0}", context.Message.Name);
        }
    }
}
