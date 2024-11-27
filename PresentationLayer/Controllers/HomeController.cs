using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Identity;
using DataLayer;

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

        public IActionResult Index([FromServices] SignInManager<ApplicationUser> sign)
        {

           // sign.SignOutAsync().Wait();
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