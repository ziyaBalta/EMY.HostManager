using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMY.HostManager.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme)]
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
