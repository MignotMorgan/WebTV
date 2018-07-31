using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CaroloTV.Services;
using CaroloTV.Models;

namespace Carolo_TV.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.News = Channels.News;
            //ViewBag.News = new List<News>();
            //ViewBag.News.Add(Channels.Find("Mangameek").WebTVService.FindNews());

            //ViewBag.Informavie = Channels.Find("Informavie").WebTVService.FindNews();
            //ViewBag.Mangameek = Channels.Find("Mangameek").WebTVService.FindNews();
            //ViewBag.Mystravely = Channels.Find("Mystravely").WebTVService.FindNews();
            //ViewBag.TutorialTV = Channels.Find("TutorialTV").WebTVService.FindNews();
            //ViewBag.Variety = Channels.Find("Variety").WebTVService.FindNews();
            //ViewBag.CaroloMusic = Channels.Find("CaroloMusic").WebTVService.FindNews();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}