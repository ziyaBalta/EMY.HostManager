using Microsoft.AspNetCore.Mvc;

namespace EMY.HostManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return PartialView();
        }
    }
}
