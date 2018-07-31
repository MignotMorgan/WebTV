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
    public class NouvellesController : Controller
    {


        private void SessionClear()
        {
            //Session["HGuid"] = "";

            Session["Channel"] = "";

            Session["HPage"] = 0;
            Session["NPage"] = 0;
            //Session["MPage"] = 0;
            //Session["PPage"] = 0;

            //Session["MKeyword"] = "";
            //Session["PKeyword"] = "";

            //Session["HCurrent"] = true;

            Session["Size"] = 10;
            Session["News"] = "";
        }
        public ActionResult NouvellesChannel(string channel)
        {
            SessionClear();
            Session["Channel"] = channel;
            return RedirectToAction("Nouvelles");
        }

        public ActionResult Nouvelles()
        {
            if (Session["Channel"] == null)
                return RedirectToAction("Index", "Home");
            string channel = (string)Session["Channel"];
            //string hname = (string)Session["HName"];
            int hPage = (int)Session["HPage"];
            int nPage = (int)Session["NPage"];
            int size = (int)Session["Size"];
            string newsTitle = (string)Session["News"];

            Channel Channel = Channels.Find(channel);
            //News news = Channel.WebTVService.FindNews();
            News news = null;
            List<News> list = Channels.News;
            foreach(News n in list)
            {
                if (n.Title == newsTitle)
                    news = n;
            }
            if (news == null)
            {

                news = new News();
                news.Channel = channel;
            }
            
            IEnumerable<Hourly> hourlys = Channel.WebTVService.FindHourlyAll();

            int htotalpage = (int)Math.Ceiling(((double)hourlys.Count() / size));
            NavPage hpage = new NavPage(hPage, htotalpage);
            int hposition = hpage.CurrentPage * size;
            hourlys = hourlys.Skip(hposition).Take(size).ToList();

            int ntotalpage = (int)Math.Ceiling(((double)list.Count() / size));
            NavPage npage = new NavPage(nPage, ntotalpage);
            int nposition = npage.CurrentPage * size;
            list = list.Skip(nposition).Take(size).ToList();

            ViewBag.Title = channel;
            ViewBag.Channel = channel;
            ViewBag.ChannelList = Channels.Names;
            //ViewBag.HName = hname;
            ViewBag.HPage = hpage;
            ViewBag.NPage = npage;
            ViewBag.Hourlys = hourlys;
            ViewBag.News = Channels.News;

            return View("Nouvelles", news);
        }

        public ActionResult NewsSelect(string value = "")
        {

            Session["News"] = value;
            return RedirectToAction("Nouvelles");
        }
        //public ActionResult HourlySelect(string value = "")
        //{

        //    Session["HGuid"] = value;
        //    return RedirectToAction("Nouvelles");
        //}
        [HttpPost]
        public ActionResult SaveNews(News news)
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);

            if (news.Channel == "")
                news.Channel = channel;

            if (ModelState.IsValid)
            {
                Channels.Add(news);
                Session["News"] = news.Title;
            }

            return RedirectToAction("Nouvelles");
        }
        public ActionResult HourlyPage(int value = 0)
        {
            Session["HPage"] = value;
            return RedirectToAction("Nouvelles");
        }
        public ActionResult NewsPage(int value = 0)
        {
            Session["NPage"] = value;
            return RedirectToAction("Nouvelles");
        }
    }
}