using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using CaroloTV.Models;

namespace CaroloTV.Services
{
    public class Channel
    {
        private string Name { get; set; }
        public Channel(string name)
        {
            Name = name;
            WebTVService = new WebTVXML(name);// new WebTVDbContext(name);
            WebTVTimer = new WebTVXML(name);// new WebTVDbContext(name);
            Timer = new Timer(OnTimedEvent, null, 0, 1000);
        }
        public IService WebTVService { get; }
        public IService WebTVTimer { get; }
        private Timer Timer { get; set; }

        public Hourly HourlyCurrent { get { return HCurrent; } }
        public string JsonCurrent { get { return JCurrent; }  }
        public Hourly HourlyNext { get { return HNext; }  }
        public string JsonNext { get { return JNext; }  }

        private Hourly HCurrent { get; set; }
        private string JCurrent { get; set; }
        private Hourly HNext { get; set; }
        private string JNext { get; set; }

        private List<Hourly> hourlys { get; set; }
        private List<Hourly> Hourlys { get { return hourlys; } }


        private void OnTimedEvent(object o)
        {
            Hourly current = null;
            Hourly next = null;
            lock (WebTVTimer)
            {
                current = WebTVTimer.Current;
                next = WebTVTimer.Next;
            }

            if (current != null && HCurrent != current)
            {
                if (HCurrent != null)
                    lock (HCurrent) { HCurrent = current; }
                else
                    HCurrent = current;
                if (HCurrent != null)
                {
                    if (JCurrent != null)
                        lock (JCurrent) { JCurrent = JsonConvert.SerializeObject(HCurrent.Media, typeof(Media), null); }
                    else
                        JCurrent = JsonConvert.SerializeObject(HCurrent.Media, typeof(Media), null);
                }
            }

            if (next != null && HNext != next)
            {
                if (HNext != null)
                    lock (HNext) { HNext = next; }
                else
                    HNext = next;
                if (HNext != null)
                {
                    if (JNext != null)
                        lock (JNext) { JNext = JsonConvert.SerializeObject(HNext.Media, typeof(Media), null); }
                    else
                        JNext = JsonConvert.SerializeObject(HNext.Media, typeof(Media), null);
                }
            }
        }

        //private void OnTimedEvent(object o)
        //{
        //    Hourly current = null;
        //    Hourly next = null;
        //    lock (WebTVTimer)
        //    {
        //        current = WebTVTimer.Current;
        //        next = WebTVTimer.Next;
        //    }

        //    if (current != null && HCurrent != current)
        //    {
        //        if (HCurrent != null)
        //            lock (HCurrent) { HCurrent = current; }
        //        else
        //            HCurrent = current;
        //        if (HCurrent != null)
        //        {
        //            if (JCurrent != null)
        //                lock (JCurrent) { JCurrent = JsonConvert.SerializeObject(HCurrent.Media, typeof(Media), null); }
        //            else
        //                JCurrent = JsonConvert.SerializeObject(HCurrent.Media, typeof(Media), null);
        //        }
        //    }

        //    if (next != null && HNext != next)
        //    {
        //        if (HNext != null)
        //            lock (HNext) { HNext = next; }
        //        else
        //            HNext = next;
        //        if (HNext != null)
        //        {
        //            if (JNext != null)
        //                lock (JNext) { JNext = JsonConvert.SerializeObject(HNext.Media, typeof(Media), null); }
        //            else
        //                JNext = JsonConvert.SerializeObject(HNext.Media, typeof(Media), null);
        //        }
        //    }
        //}
    }
}
