using EMY.HostManager.Bussines;
using EMY.HostManager.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMY.HostManager.Web.Controllers
{
    public class TemplateController : Controller
    {
        HostManagerFactory factory;

        public TemplateController()
        {
            this.factory = new HostManagerFactory();
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "TemplateShow,TemplateFull,AdminFull")]
        public async Task<IActionResult> Index()
        {
            var result = await factory.Templates.GetAll();
            return View(result);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "TemplateAdd,TemplateFull,AdminFull")]
        public IActionResult Create()
        {
            ViewBag.Error = false;
            ViewBag.ErrorMessage = "";
            Template newtemplate = new Template();
            return View("CreateOrUpdate", newtemplate);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "TemplateUp,TemplateFull,AdminFull")]
        public async Task<IActionResult> Edit(int TemplateID)
        {
            ViewBag.Error = false;
            ViewBag.ErrorMessage = "";
            Template template = await factory.Templates.GetByTeplateID(TemplateID);
            return View("CreateOrUpdate", template);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "TemplateShow,TemplateFull,AdminFull")]
        public async Task<IActionResult> Details(int TemplateID)
        {
            Template template = await factory.Templates.GetByTeplateID(TemplateID);
            if (template == null) return NotFound();
            return View(template);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "TemplateDel,TemplateFull,AdminFull")]
        public async Task<IActionResult> Delete(int TemplateID)
        {
            Template template = await factory.Templates.GetByTeplateID(TemplateID);
            if (template == null) return NotFound();
            return View(template);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme, Roles = "TemplateDel,TemplateFull,AdminFull")]
        public async Task<IActionResult> DeleteConfirmed(int TemplateID)
        {
            Template template = await factory.Templates.GetByTeplateID(TemplateID);
            if (template == null) return NotFound();
            await factory.Templates.Delete(template, int.Parse(User.Identity.Name));
            return Redirect("Index");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = SystemStatics.DefaultScheme)]
        public async Task<IActionResult> Save(Template template)
        {
            ViewBag.Error = true;

            Template foundtemplate = await factory.Templates.GetByTemplateName(template.TemplateName);
            if (template.TemplateID == 0)
            {
                if (!(User.IsInRole("TemplateAdd") || User.IsInRole("AdminFull") || User.IsInRole("TemplateAll")))
                    return Unauthorized();

                if (foundtemplate != null)
                {
                    ViewBag.ErrorMessage = "This name already used! Please write different name!";
                    return View("CreateOrUpdate", template);
                }

                await factory.Templates.Add(template, int.Parse(User.Identity.Name));
            }
            else
            {
                if (!(User.IsInRole("TemplateUp") || User.IsInRole("AdminFull") || User.IsInRole("TemplateAll")))
                    return Unauthorized();
                if (foundtemplate != null && foundtemplate.TemplateID != template.TemplateID)
                {
                    ViewBag.ErrorMessage = "This name already used! Please write different name!";
                    return View("CreateOrUpdate", template);
                }
                else
                {
                    Template existTemplate = await factory.Templates.GetByTeplateID(template.TemplateID);
                    if (existTemplate == null) return NotFound();
                    existTemplate.TemplateName = template.TemplateName;
                    existTemplate.TemplateCode = template.TemplateCode;
                    await factory.Templates.Update(existTemplate, int.Parse(User.Identity.Name));
                }
            }

            return Redirect("Index");
        }


    }
}
