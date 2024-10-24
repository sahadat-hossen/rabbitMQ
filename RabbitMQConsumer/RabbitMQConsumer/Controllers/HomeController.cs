using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using RabbitMQConsumer.Models;

namespace RabbitMQConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly RabbitMQConsumerService _consumerService;

        public HomeController(RabbitMQConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        public IActionResult Index()
        {
            var message = _consumerService.GetMessage();
            if (string.IsNullOrEmpty(message)) {
            return NotFound("No new Messages");
            }
            return Ok(message);
        }

       
    }
}
