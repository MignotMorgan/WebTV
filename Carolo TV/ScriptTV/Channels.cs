using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using CaroloTV.Models;

namespace CaroloTV.Services
{
    public static class Channels
    {
        private static List<string> C_Names { get; set; }
        public static List<string> Names
        {
            get
            {
                if (C_Names == null)
                {
                    C_Names = new List<string>
                    {
                        "Informavie",
                        "Mangameek",
                        "Mystravely",
                        "TutorialTV",
                        "Variety",
                        "CaroloMusic"
                    };
                }
                return C_Names;
            }
        }
        private static List<News> C_News { get; set; }
        public static List<News> News
        {
            get
            {
                if (C_News == null)
                {
                    C_News = ServiceChannel.FindAllNews().ToList();
                }
                return C_News;
            }
        }
        public static void Add(News news)
        {
            ServiceChannel.Save(news);
            lock (C_News)
            {
                C_News = ServiceChannel.FindAllNews().ToList();
            }
        }
        private static IServiceChannels C_ServiceChannel { get; set; }
        public static IServiceChannels ServiceChannel
        { get
            {
                if(C_ServiceChannel == null)
                {
                    C_ServiceChannel = new ChannelsXML();
                }
                return C_ServiceChannel;
            }
        }

        private static Channel C_divers { get; set; }
        public static Channel Divers
        {
            get
            {
                if (C_divers == null)
                    C_divers = new Channel("Divers");
                return C_divers;
            }
        }
        private static Channel C_informavie { get; set; }
        public static Channel Informavie
        {
            get
            {
                if (C_informavie == null)
                    C_informavie = new Channel("Informavie");
                return C_informavie;
            }
        }
        private static Channel C_mangameek { get; set; }
        public static Channel Mangameek
        {
            get
            {
                if (C_mangameek == null)
                    C_mangameek = new Channel("Mangameek");
                return C_mangameek;
            }
        }
        private static Channel C_mystravely { get; set; }
        public static Channel Mystravely
        {
            get
            {
                if (C_mystravely == null)
                    C_mystravely = new Channel("Mystravely");
                return C_mystravely;
            }
        }
        private static Channel C_tutorialTV { get; set; }
        public static Channel TutorialTV
        {
            get
            {
                if (C_tutorialTV == null)
                    C_tutorialTV = new Channel("TutorialTV");
                return C_tutorialTV;
            }
        }
        private static Channel C_variety { get; set; }
        public static Channel Variety
        {
            get
            {
                if (C_variety == null)
                    C_variety = new Channel("Variety");
                return C_variety;
            }
        }

        public static Channel Find(string name)
        {
            if (name == "Divers") return Divers;
            else if (name == "Informavie") return Informavie;
            else if (name == "Mangameek") return Mangameek;
            else if (name == "Mystravely") return Mystravely;
            else if (name == "TutorialTV") return TutorialTV;
            else if (name == "Variety") return Variety;
            else return Divers;
        }
        public static Hourly HourlyCurrent(string name)
        {
            Channel channel = Find(name);
            return channel.HourlyCurrent;
        }
        public static Hourly HourlyNext(string name)
        {
            Channel channel = Find(name);
            return channel.HourlyNext;

        }
        public static string JsonCurrent(string name)
        {
            Channel channel = Find(name);
            return channel.JsonCurrent;
        }
        public static string JsonNext(string name)
        {
            Channel channel = Find(name);
            return channel.JsonNext;
        }
    }
}