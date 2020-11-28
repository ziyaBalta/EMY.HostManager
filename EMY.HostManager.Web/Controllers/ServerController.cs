using EMY.HostManager.Bussines;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMY.HostManager.Web.Controllers
{
    public class ServerController : Controller
    {
        HostManagerFactory factory;
        public ServerController()
        {
            this.factory = new HostManagerFactory();
        }
        public async Task<IActionResult> Index()
        {
            var result = await factory.ServerInformations.GetServerList();
            return View(result);
        }
    }
}
