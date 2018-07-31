using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CaroloTV.Models;
using CaroloTV.Services;
using System.Web.Mvc;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaroloTV.Controllers
{
    public class WebTVController : Controller
    {
        public ActionResult WebTVChannel(string channel)
        {
            Session["Channel"] = channel;
            return RedirectToAction("WebTV");
        }
        public ActionResult WebTV()
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);
            IEnumerable<Hourly> hourlys = Channel.WebTVService.FindHourlyAll();

            ViewData["Title"] = channel;
            ViewBag.Channel = channel;

            return View("WebTV", hourlys);
        }

        [HttpPost]
        public JsonResult NextMedia()
        {
            string channel = (string)Session["Channel"];
            string json = Channels.JsonNext(channel);
            return Json(json);
        }

        [HttpPost]
        public JsonResult FirstMedia()
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);
            Hourly current = Channels.HourlyCurrent(channel);
            Hourly next = Channels.HourlyNext(channel);
            Media currentmedia = current.Media;
            Media nextmedia = next.Media;
            int time = (int)(DateTime.Now - current.Date).TotalSeconds;
            currentmedia.StartSeconds += time;
            currentmedia.Duration = currentmedia.Duration - time;
            string json = JsonConvert.SerializeObject(new { Current = currentmedia, Next = nextmedia });

            return Json(json );
        }
    }
}
