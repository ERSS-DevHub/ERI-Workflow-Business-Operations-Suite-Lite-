using System.Diagnostics;
using ERI_Workflow___Business_Operations_Suite__Lite_.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
