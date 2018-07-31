using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
using CaroloTV.Models;
using CaroloTV.Services;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaroloTV.Controllers
{
    public class OfficesController : Controller
    {
        public ActionResult Avis()
        {
            return View("Avis");
        }
        public ActionResult CreateWebTV()
        {
            //if (User.Identity.IsAuthenticated)
                return View("CreateWebTV");
            //else
            //    return RedirectToAction("Index", "Home");
        }
        public ActionResult Finances()
        {
            return View("Finances");
        }
        public ActionResult Journalisme()
        {
            return View("Journalisme");
        }
        public ActionResult Publicite()
        {
            return View("Publicite");
        }
        public ActionResult Suggestion()
        {
            return View("Suggestion");
        }
    }
}
