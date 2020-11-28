using EMY.HostManager.Bussines;
using EMY.HostManager.Entities;
using EMY.HostManager.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMY.HostManager.Web.Controllers
{
    public class AccountController : Controller
    {
        HostManagerFactory factory;
        public AccountController()
        {
            this.factory = new HostManagerFactory();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ViewBag.Error = false;
            ViewBag.ErrorMessage = "";
            LoginUserViewModel login = new LoginUserViewModel();
            return PartialView(login);
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserViewModel login)
        {
            CheckAdminUser();
            var res = await factory.Users.CheckLoginUser(login.UserName, login.Password);
            if (res.IsSuccess && res.resultType == 1)
            {
                var loggedinUser = (User)res.Data;
                List<Claim> mainClaim = new List<Claim>() {
                    new Claim(ClaimTypes.Name, loggedinUser.GetName)
                };

                var authList = await factory.Users.GetAllRoles(loggedinUser.UserID);

                foreach (var role in authList)
                {
                    var rl = new Claim(ClaimTypes.Role, role);
                    mainClaim.Add(rl);
                }

                ClaimsIdentity MainIdentity = new ClaimsIdentity(mainClaim, "EmyIdentity");

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(MainIdentity);

                await HttpContext.SignInAsync(
                    SystemStatics.DefaultScheme,
                    userPrincipal
                    ,new AuthenticationProperties{IsPersistent = login.RememberMe}
                    );
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = res.Message;
                ViewBag.Error = true;
                return PartialView(login);
            }


        }

        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme)]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(scheme: SystemStatics.DefaultScheme);
            return RedirectToAction("Login");
        }

        async void CheckAdminUser()
        {
            string UserName = "EnesMY";
            string Password = "123456";
            string Role = "Admin";
            var res = await factory.Users.CheckLoginUser(UserName, Password);
            if (res.resultType == -1)
            {
                var user = new User
                {

                    Name = "Enes Murat",
                    LastName = "YILDIRIM",
                    UserName = UserName,
                    Password = Password,
                    IsActive = true,
                    IsDeleted = false,
                    UserCode = 1
                };


                await factory.Users.Add(user, 0);

                UserRole newRole = new UserRole()
                {
                    UserID = user.UserID,
                    FormName = Role,
                    AuthorizeType = AuthType.Full
                };
                await factory.Users.AddRole(newRole, user.UserID);
            }

        }


    }
}
