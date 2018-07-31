using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaroloTV.Models;
using CaroloTV.Services;
using System.Web.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CaroloTV.Controllers
{
    public class CreateController : Controller
    {
        private void SessionClear()
        {
            Session["HName"] = "";
            Session["PName"] = "";

            Session["Channel"] = "Informavie";

            Session["HPage"] = 0;
            Session["MPage"] = 0;
            Session["PPage"] = 0;

            Session["MKeyword"] = "";
            Session["PKeyword"] = "";

            Session["HCurrent"] = true;

            Session["Size"] = 10;
        }
        public ActionResult CreateChannel(string channel)
        {
            SessionClear();
            Session["Channel"] = channel;
            return RedirectToAction("Proprietary");
        }

        public ActionResult Proprietary()
        {
            if (Session["Channel"] == null)
                return RedirectToAction("Index", "Home");
            string channel = (string)Session["Channel"];
            string pname = (string)Session["PName"];
            int pPage = (int)Session["PPage"];
            string pKeyword = (string)Session["PKeyword"];
            int size = (int)Session["Size"];

            Channel Channel = Channels.Find(channel);
            Proprietary proprietary = Channel.WebTVService.FindProprietaryByName(pname);
            if (proprietary == null) { proprietary = new Proprietary(); }

            IEnumerable<Proprietary> prop = Channel.WebTVService.FindProprietaryAll()
                .Where(p => p.Name.Contains(pKeyword));

            int ptotalpage = (int)Math.Ceiling(((double)prop.Count() / size));
            NavPage ppage = new NavPage(pPage, ptotalpage);
            int position = ppage.CurrentPage * size;

            ViewBag.Channel = channel;
            ViewBag.PPage = ppage;
            ViewBag.Proprietarys = prop
                .Skip(position)
                .Take(size)
                .ToList();
            ViewBag.PKeyword = pKeyword;

            return View("Proprietary", proprietary);
        }
        public ActionResult ProprietaryClear()
        {
            Session["PName"] = "";
            return RedirectToAction("Proprietary");
        }
        public ActionResult ProprietaryPKeyword(string value = "")
        {
            Session["PKeyword"] = value;
            return RedirectToAction("Proprietary");
        }
        public ActionResult ProprietarySelect(string value = "")
        {
            Session["PName"] = value;
            return RedirectToAction("Proprietary");
        }
        public ActionResult ProprietaryPage(int value = 0)
        {
            Session["PPage"] = value;
            return RedirectToAction("Proprietary");
        }
        public ActionResult ProprietaryToMedia(string value = "")
        {
            Session["PName"] = value;
            Session["MName"] = "";
            return RedirectToAction("Media");
        }

        [HttpPost]
        public ActionResult SaveProprietary(Proprietary proprietary)
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);

            if (ModelState.IsValid)
                Channel.WebTVService.Save(proprietary);

            return RedirectToAction("Proprietary");
        }

        public ActionResult DeleteProprietary(string value = "")
        {
            string channel = (string)Session["Channel"];
            string pname = (string)Session["PName"];
            Channel Channel = Channels.Find(channel);
            Proprietary proprietary = Channel.WebTVService.FindProprietaryByName(value);
            if (ModelState.IsValid)
            {
                if (proprietary != null)
                {
                    Channel.WebTVService.Delete(proprietary);
                    if (pname == value)
                        Session["PName"] = "";
                }
            }

            return RedirectToAction("Proprietary");
        }

        public ActionResult Media()
        {
            if (Session["Channel"] == null)
                return RedirectToAction("Index", "Home");
            string channel = (string)Session["Channel"];
            string pname = (string)Session["PName"];
            string mname = (string)Session["MName"];
            int mPage = (int)Session["MPage"];
            string mKeyword = (string)Session["MKeyword"];
            int size = (int)Session["Size"];

            Channel Channel = Channels.Find(channel);
            Proprietary proprietary = Channel.WebTVService.FindProprietaryByName(pname);
            Media media = Channel.WebTVService.FindMediaByName(mname, pname);
            if (media == null) { media = new Media { ProprietaryName = pname}; }

            IEnumerable<Media> med = Channel.WebTVService.FindMediaByProprietary(pname).Where(p => p.Name.Contains(mKeyword));

            int mtotalpage = (int)Math.Ceiling(((double)med.Count() / size));
            NavPage mpage = new NavPage(mPage, mtotalpage);
            int mposition = mpage.CurrentPage * size;

            ViewBag.Videoplayer = new VideoPlayer();
            ViewBag.Channel = channel;
            ViewBag.MPage = mpage;
            ViewBag.Medias = med
                .Skip(mposition)
                .Take(size)
                .ToList();
            ViewBag.Keyword = mKeyword;
            ViewBag.PName = pname;
            ViewBag.Proprietary = proprietary;

            return View("Media", media);
        }
        public ActionResult MediaClear()
        {
            Session["MName"] = "";
            return RedirectToAction("Media");
        }
        public ActionResult MediaMKeyword(string value = "")
        {
            Session["MKeyword"] = value;
            Session["MPage"] = 0;
            return RedirectToAction("Media");
        }
        public ActionResult MediaSelect(string value = "")
        {

            Session["MName"] = value;
            return RedirectToAction("Media");
        }
        public ActionResult MediaPage(int value = 0)
        {
            Session["MPage"] = value;
            return RedirectToAction("Media");
        }

        [HttpPost]
        public ActionResult SaveMedia(Media media)
        {
            string channel = (string)Session["Channel"];
            Channel Channel = Channels.Find(channel);

            if (ModelState.IsValid)
                Channel.WebTVService.Save(media);

            return RedirectToAction("Media");
        }

        public ActionResult DeleteMedia(string value = "")
        {
            string channel = (string)Session["Channel"];
            string pname = (string)Session["PName"];
            Channel Channel = Channels.Find(channel);
            Media media = Channel.WebTVService.FindMediaByName(value, pname);
            if (ModelState.IsValid)
            {
                if (media != null)
                {
                    Channel.WebTVService.Delete(media);
                    if ((string)Session["MName"] == value)
                        Session["MName"] = "";
                }
            }

            return RedirectToAction("Media");
        }
    }
}
