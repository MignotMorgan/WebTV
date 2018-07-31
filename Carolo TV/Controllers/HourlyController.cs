using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
using CaroloTV.Models;
using CaroloTV.Services;
using System.Web.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaroloTV.Controllers
{
    public class HourlyController : Controller
    {
        private void SessionClear()
        {
            Session["HName"] = "";
            Session["PName"] = "";

            Session["Channel"] = "";

            Session["HPage"] = 0;
            Session["MPage"] = 0;
            Session["PPage"] = 0;

            Session["MKeyword"] = "";
            Session["PKeyword"] = "";

            Session["HCurrent"] = true;

            Session["Size"] = 10;
        }
        public ActionResult HourlyChannel(string channel)
        {
            SessionClear();
            Session["Channel"] = channel;
            return RedirectToAction("Hourly");
        }

        public ActionResult Hourly()
        {
            if (Session["Channel"] == null)
                return RedirectToAction("Index", "Home");
            string hname = (string)Session["HName"];
            string pname = (string)Session["PName"];
            string channel = (string)Session["Channel"];
            int hPage = (int)Session["HPage"];
            int mPage = (int)Session["MPage"];
            int pPage = (int)Session["PPage"];
            string mKeyword = (string)Session["MKeyword"];
            bool hcurrent = (bool)Session["hcurrent"];
            int size = (int)Session["Size"];
            
            Channel Channel = Channels.Find(channel);
            Proprietary proprietary = Channel.WebTVService.FindProprietaryByName(pname);
            IEnumerable<Hourly> hourlys;

            if (hcurrent)
                hourlys = Channel.WebTVService.FindHourlyAll();
            else
                hourlys = Channel.WebTVService.FindHourly(hname);

            IEnumerable<Proprietary> proprietarys = Channel.WebTVService.FindProprietaryAll();
            IEnumerable<Media> medias;
            IEnumerable<string> hourlysList = Channel.WebTVService.FindHourlyList();

            if (proprietary.Name != "Anonyme")
                medias = Channel.WebTVService.FindMediaByProprietary(pname);
            else
                medias = Channel.WebTVService.FindMediaAll();

            medias = medias.Where(m => m.Name.Contains(mKeyword)).ToList();
            int mtotalpage = (int)Math.Ceiling(((double)medias.Count() / size));
            NavPage mpage = new NavPage(mPage, mtotalpage);
            int mposition = mpage.CurrentPage * size;
            medias = medias.Skip(mposition).Take(size).ToList();

            int htotalpage = (int)Math.Ceiling(((double)hourlys.Count() / size));
            NavPage hpage = new NavPage(hPage, htotalpage);
            int hposition = hpage.CurrentPage * size;
            hourlys = hourlys.Skip(hposition).Take(size).ToList();

            int ptotalpage = (int)Math.Ceiling(((double)proprietarys.Count() / size));
            NavPage ppage = new NavPage(pPage, ptotalpage);
            int proprietaryposition = ppage.CurrentPage * size;
            proprietarys = proprietarys.Skip(proprietaryposition).Take(size).ToList();

            ViewBag.Title = channel;
            ViewBag.Channel = channel;
            ViewBag.PName = pname;
            ViewBag.HName = hname;
            ViewBag.HPage = hpage;
            ViewBag.MPage = mpage;
            ViewBag.PPage = ppage;
            ViewBag.MKeyword = mKeyword;
            ViewBag.HCurrent = hcurrent;
            ViewBag.Proprietarys = proprietarys;
            ViewBag.Medias = medias;
            ViewBag.HourlysList = hourlysList;

            return View("Hourly", hourlys);
        }
        public ActionResult HourlyCurrent()
        {
            Session["hcurrent"] = !((bool)Session["hcurrent"]);
            return RedirectToAction("Hourly");
        }
        public ActionResult HourlyPage(int value = 0)
        {
            Session["HPage"] = value;
            return RedirectToAction("Hourly");
        }
        public ActionResult HourlyMediaPage(int value = 0)
        {
            Session["MPage"] = value;
            return RedirectToAction("Hourly");
        }
        public ActionResult HourlyProprietaryPage(int value = 0)
        {
            Session["PPage"] = value;
            return RedirectToAction("Hourly");
        }
        public ActionResult HourlyMKeyword(string value = "")
        {
            Session["MKeyword"] = value;
            return RedirectToAction("Hourly");
        }
        public ActionResult HourlyClearProprietary()
        {
            Session["PName"] = "";
            return RedirectToAction("Hourly");
        }
        public ActionResult HourlySelectProprietary(string value = "")
        {
            Session["PName"] = value;
            return RedirectToAction("Hourly");
        }
        public ActionResult HourlySelect(string value = "")
        {
            Session["HName"] = value;
            Session["HCurrent"] = false;
            return RedirectToAction("Hourly");
        }

        public ActionResult AddMedia(string mname, string pname)
        {
            string hname = (string)Session["HName"];
            string channel = (string)Session["Channel"];
            bool hcurrent = (bool)Session["hcurrent"];

            Channel Channel = Channels.Find(channel);
            Channel.WebTVService.NewHourly(mname, pname, hname, hcurrent);

            return RedirectToAction("Hourly");
        }

        public ActionResult DeleteHourly(string value="")
        {
            string channel = (string)Session["Channel"];
            string hname = (string)Session["HName"];
            bool hcurrent = (bool)Session["hcurrent"];
            Channel Channel = Channels.Find(channel);
            Channel.WebTVService.DeleteHourly(value, hname, hcurrent);

            return RedirectToAction("Hourly");
        }

        public ActionResult ActualiserHourly()
        {
            string channel = (string)Session["Channel"];
            string hname = (string)Session["HName"];
            bool hcurrent = (bool)Session["hcurrent"];
            Channel Channel = Channels.Find(channel);
            Channel.WebTVService.Reorganize(DateTime.Now, hname, hcurrent);

            return RedirectToAction("Hourly");
        }

        public ActionResult CreateHourly(string value = "")
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);
            Channel.WebTVService.CreateHourly(value);

            return RedirectToAction("Hourly");
        }

        public ActionResult RemplaceWebTV(string value = "")
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);
            Channel.WebTVService.PlaceWebTV(value);

            return RedirectToAction("Hourly");
        }

        public ActionResult AjouteWebTV(string value = "")
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);
            Channel.WebTVService.AjouteWebTV(value);

            return RedirectToAction("Hourly");
        }



        //public ActionResult NouvellesChannel(string channel)
        //{
        //    SessionClear();
        //    Session["Channel"] = channel;
        //    return RedirectToAction("Nouvelles");
        //}

        //public ActionResult Nouvelles()
        //{
        //    if (Session["Channel"] == null)
        //        return RedirectToAction("Index", "Home");
        //    string channel = (string)Session["Channel"];
        //    //string hname = (string)Session["HName"];
        //    int hPage = (int)Session["HPage"];
        //    int size = (int)Session["Size"];

        //    Channel Channel = Channels.Find(channel);
        //    News news = Channel.WebTVService.FindNews();
        //    IEnumerable<Hourly> hourlys = Channel.WebTVService.FindHourlyAll();

        //    int htotalpage = (int)Math.Ceiling(((double)hourlys.Count() / size));
        //    NavPage hpage = new NavPage(hPage, htotalpage);
        //    int hposition = hpage.CurrentPage * size;
        //    hourlys = hourlys.Skip(hposition).Take(size).ToList();

        //    ViewBag.Title = channel;
        //    ViewBag.Channel = channel;
        //    //ViewBag.HName = hname;
        //    ViewBag.HPage = hpage;
        //    ViewBag.Hourlys = hourlys;

        //    return View("Nouvelles", news);
        //}

        //public ActionResult NewsSelect(string value = "")
        //{

        //    Session["HName"] = value;
        //    return RedirectToAction("Nouvelles");
        //}
        //[HttpPost]
        //public ActionResult SaveNews(News news)
        //{
        //    string channel = (string)Session["Channel"];
        //    Channel Channel = Channels.Find(channel);

        //    if (news.Channel == "")
        //        news.Channel = channel;

        //    if (ModelState.IsValid)
        //        Channel.WebTVService.Save(news);

        //    return RedirectToAction("Nouvelles");
        //}

    }
}
