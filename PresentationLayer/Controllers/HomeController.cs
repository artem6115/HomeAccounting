using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLayer.Services;
namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GeneratorEntitiesService Generator;


        public HomeController(ILogger<HomeController> logger, GeneratorEntitiesService gen)
        {
            _logger = logger;
            Generator = gen;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Generate()
        {
            Generator.Generate(3, 100, 8);
            return RedirectToAction("get","Account");
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}