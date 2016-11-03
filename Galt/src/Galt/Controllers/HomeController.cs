using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Galt.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Galt allows you to manage all the dependencies from your projets and be aware of every issue or new version of a package.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Team Galt";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
