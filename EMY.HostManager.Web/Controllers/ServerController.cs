using EMY.HostManager.Bussines;
using EMY.HostManager.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMY.HostManager.Web.Controllers
{
    [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme)]
    public class ServerController : Controller
    {
        HostManagerFactory factory;

        public ServerController()
        {
            this.factory = new HostManagerFactory();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "ServerShow,ServerFull,AdminFull")]
        public async Task<IActionResult> Index()
        {
            var result = await factory.ServerInformations.GetServerList();


            return View(result);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "ServerAdd,ServerFull,AdminFull")]
        public IActionResult Create()
        {
            ViewBag.Error = false;
            ViewBag.ErrorMessage = "";
            ServerInformation newserver = new ServerInformation();
            return View("CreateOrUpdate", newserver);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "ServerUp,ServerFull,AdminFull")]
        public async Task<IActionResult> Edit(int ServerInformationID)
        {
            ViewBag.Error = false;
            ViewBag.ErrorMessage = "";
            ServerInformation existServer = await factory.ServerInformations.GetServerInformationByServerInformationID(ServerInformationID);
            if (existServer == null)
                return NotFound();
            return View("CreateOrUpdate", existServer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ServerInformation server)
        {
            ViewBag.Error = true;

            var ServerNameUniqeControl = await factory.ServerInformations.GetServerByName(server.ServerName);

            if (server.ServerInformationID == 0)
            {
                if (!(User.IsInRole("ServerAdd") || User.IsInRole("AdminFull") || User.IsInRole("ServerAll")))
                    return Unauthorized();

                if (ServerNameUniqeControl != null)
                {
                    ViewBag.ErrorMessage = "Server name already exist in database!";
                    return View("CreateOrUpdate", server);

                }
                else
                {
                    await factory.ServerInformations.Add(server, int.Parse(User.Identity.Name));
                }
            }
            else
            {
                if (!(User.IsInRole("ServerUp") || User.IsInRole("AdminFull") || User.IsInRole("ServerAll")))
                    return Unauthorized();
                if (ServerNameUniqeControl != null && ServerNameUniqeControl.ServerInformationID != server.ServerInformationID)
                {
                    ViewBag.ErrorMessage = "Server name already exist in database!";
                    return View("CreateOrUpdate", server);
                }
                else
                {
                    var foundServer = await factory.ServerInformations.GetServerInformationByServerInformationID(server.ServerInformationID);
                    foundServer.ServerName = server.ServerName;
                    foundServer.ServerAdress = server.ServerAdress;
                    foundServer.ServerType = server.ServerType;
                    foundServer.Port = server.Port;
                    foundServer.UserName = server.UserName;
                    foundServer.Password = string.IsNullOrEmpty(server.Password) ? foundServer.Password : server.Password;
                    await factory.ServerInformations.Update(foundServer, int.Parse(User.Identity.Name));
                }
            }
            return Redirect("Index");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "ServerShow,ServerFull,AdminFull")]
        public async Task<IActionResult> Details(int ServerInformationID)
        {
            ServerInformation existServer = await factory.ServerInformations.GetServerInformationByServerInformationID(ServerInformationID);
            if (existServer == null)
                return NotFound();
            return View(existServer);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "ServerDel,ServerFull,AdminFull")]
        public async Task<IActionResult> Delete(int ServerInformationID)
        {
            ServerInformation existServer = await factory.ServerInformations.GetServerInformationByServerInformationID(ServerInformationID);
            if (existServer == null)
                return NotFound();
            return View(existServer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "ServerDel,ServerFull,AdminFull")]
        public async Task<IActionResult> DeleteConfirmed(int ServerInformationID)
        {
            ServerInformation existServer = await factory.ServerInformations.GetServerInformationByServerInformationID(ServerInformationID);
            if (existServer == null)
                return NotFound();

            await factory.ServerInformations.Delete(existServer, int.Parse(User.Identity.Name));

            return Redirect("Index");
        }

    }
}
