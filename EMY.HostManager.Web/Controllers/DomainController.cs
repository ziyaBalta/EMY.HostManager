using EMY.HostManager.Bussines;
using EMY.HostManager.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMY.HostManager.Web.Controllers
{
    public class DomainController : Controller
    {
        HostManagerFactory factory;
        public DomainController()
        {
            this.factory = new HostManagerFactory();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var domains = await factory.Domains.GetDomainList();
            return View(domains);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Error = false;
            ViewBag.ErrorMessage = "";
            DomainInformation newDomain = new DomainInformation();
            return View("CreateOrUpdate", newDomain);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int DomainInformationID)
        {
            ViewBag.Error = false;
            ViewBag.ErrorMessage = "";
            DomainInformation domain = await factory.Domains.GetDomainInformationByDomainInformationID(DomainInformationID);
            if (domain == null)
                return NotFound();
            return View("CreateOrUpdate", domain);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int DomainInformationID)
        {
            DomainInformation domain = await factory.Domains.GetDomainInformationByDomainInformationID(DomainInformationID);
            if (domain == null)
                return NotFound();
            return View(domain);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int DomainInformationID)
        {
            DomainInformation domain = await factory.Domains.GetDomainInformationByDomainInformationID(DomainInformationID);
            if (domain == null)
                return NotFound();
            return View(domain);
        }

        [HttpGet]
        public async Task<IActionResult> Run(int DomainInformationID)
        {
            DomainInformation domain = await factory.Domains.GetDomainInformationByDomainInformationID(DomainInformationID);
            if (domain == null)
                return NotFound();
            return View(domain);
        }

        [HttpGet, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int DomainInformationID)
        {
            DomainInformation domain = await factory.Domains.GetDomainInformationByDomainInformationID(DomainInformationID);
            if (domain == null)
                return NotFound();

            await factory.Domains.Delete(domain, int.Parse(User.Identity.Name));
            return Redirect("Index");

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DomainInformation domain)
        {
            ViewBag.Error = true;

            var DomainNameUniqeControl = await factory.Domains.GetDomainByName(domain.DomainName);

            if (domain.DomainInformationID == 0)
            {
                if (!(User.IsInRole("DomainAdd") || User.IsInRole("AdminFull") || User.IsInRole("DomainAll")))
                    return Unauthorized();

                if (DomainNameUniqeControl != null)
                {
                    ViewBag.ErrorMessage = "Domain name already exist in database!";
                    return View("CreateOrUpdate", domain);

                }
                else
                {
                    await factory.Domains.Add(domain, int.Parse(User.Identity.Name));
                }
            }
            else
            {
                if (!(User.IsInRole("DomainUp") || User.IsInRole("AdminFull") || User.IsInRole("DomainAll")))
                    return Unauthorized();
                if (DomainNameUniqeControl.DomainInformationID != domain.DomainInformationID)
                {
                    ViewBag.ErrorMessage = "Domain name already exist in database!";
                    return View("CreateOrUpdate", domain);
                }
                else
                {
                    var foundDomain = await factory.Domains.GetDomainInformationByDomainInformationID(domain.DomainInformationID);
                    foundDomain.DomainName = domain.DomainName;
                    foundDomain.DomainAdress = domain.DomainAdress;

                    await factory.Domains.Update(foundDomain, int.Parse(User.Identity.Name));
                }
            }
            return Redirect("Index");
        }
    }
}
