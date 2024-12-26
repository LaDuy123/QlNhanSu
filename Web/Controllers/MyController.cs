using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Mapbox()
        {
            return View();
        }
    }
}
