using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using RabbitMQPublisher.Models;

namespace RabbitMQPublisher.Controllers
{
    public class HomeController : Controller
    {
      private readonly RabbitMQService _rabbitMQService;

        public HomeController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public IActionResult Index()
        {
            _rabbitMQService.SendMessage("myQueue", "Hello Rabbit MQ!");
            return View();
        }

       
    }
}
