using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Models;

namespace MassTransit.Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IBus _bus;
        private readonly IPublishEndpoint _publishEndpoint;

        public MessageController(ILogger<MessageController> logger, IBus bus, 
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _bus = bus;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("send-fanout/{msg}")]
        public async Task<IActionResult> SendFanout(string msg)
        {
            var order = new Order()
            {
                Name = msg
            };

            await _bus.Publish<Order>(order);

            return Ok("Message Sent");
        }

        [HttpGet("send-direct/{topic}/{msg}")]
        public async Task<IActionResult> SendDirect(string topic, string msg)
        {
            var order = new Order()
            {
                Name = msg
            };

            //await _publishEndpoint.Publish(topic, order);

            await _bus.Publish<Order>(order);

            return Ok("Message Sent");
        }
    }
}
