using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CaroloTV.Models;
using System.Text;
using System.Xml;
using System.IO;

namespace CaroloTV.Services
{
    public class ChannelsXML : IServiceChannels
    {
        //public List<News> News = new List<News>();
        private Encoding Encoding = Encoding.UTF8;
        private string Addresse_CaroloTV { get; set; } = @"C:\inetpub\SandBox\CaroloTV\";
        public News FindNews(string filename)
        {
            return XmlLoadNews(PathNews(filename));
        }
        public IEnumerable<News> FindAllNews()
        {
            List<News> list = new List<News>();

            DirectoryInfo directory = Directory.CreateDirectory(PathNewsDirectory());
            foreach (FileInfo f in directory.GetFiles())
            {
                News news = FindNews(Path.GetFileNameWithoutExtension(f.FullName));
                //if (!news.Active)
                //    continue;

                Channel channel = Channels.Find(news.Channel);

                List<Hourly> hourlys = channel.WebTVService.FindHourlyAll().ToList();
                foreach (Hourly h in hourlys)
                {
                    if (h.Guid == news.HourlyGuid)
                    {
                        news.Hourly = h;
                        break;
                    }
                }
                if (news.Hourly != null)// && news.Hourly.Date > DateTime.Now )
                    list.Add(news);
            }
            return list;
        }
        public void Save(News news)
        {
            XmlSaveNews(news, PathNews(news.Title));
        }
        private string PathNewsDirectory()
        {
            return Addresse_CaroloTV + @"News\";
        }
        private string PathNews(string namefile)
        {
            return PathNewsDirectory() + namefile + @".xml";
        }
        private void XmlSaveNews(News news, string path)
        {
            //string pathDirectory = PathNewsDirectory();
            XmlTextWriter writer = new XmlTextWriter(path, Encoding);

            writer.WriteStartDocument(true);
            writer.WriteStartElement("Media");

            writer.WriteElementString("Channel", news.Channel);
            writer.WriteElementString("Active", news.Active.ToString());
            writer.WriteElementString("HourlyGuid", news.HourlyGuid);
            writer.WriteElementString("StartSeconds", news.StartSeconds.ToString());
            writer.WriteElementString("Duration", news.Duration.ToString());
            writer.WriteElementString("Image", news.Image);
            writer.WriteElementString("Title", news.Title);
            writer.WriteElementString("Resume", news.Resume);
            writer.WriteElementString("Description", news.Description);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
        private News XmlLoadNews(string path)
        {
            News news = new News();
            if (!File.Exists(path))
            {
                //news.Hourly = NullableHourly;
                return news;
            }
            XmlReader reader = XmlReader.Create(path);

            reader.Read();
            if (reader.NodeType == XmlNodeType.XmlDeclaration) reader.Read();
            if (reader.IsStartElement()) reader.Read();

            news.Channel = reader.ReadElementContentAsString();
            news.Active = Boolean.Parse(reader.ReadElementContentAsString());
            news.HourlyGuid = reader.ReadElementContentAsString();
            news.StartSeconds = reader.ReadElementContentAsInt();
            news.Duration = reader.ReadElementContentAsInt();
            news.Image = reader.ReadElementContentAsString();
            news.Title = reader.ReadElementContentAsString();
            news.Resume = reader.ReadElementContentAsString();
            news.Description = reader.ReadElementContentAsString();

            reader.Close();
            Channel channel = Channels.Find(news.Channel);
            List<Hourly> hourlys = channel.WebTVService.FindHourlyAll().ToList();
            foreach (Hourly h in hourlys)
                if (h.Guid == news.HourlyGuid)
                    news.Hourly = h;

            return news;
        }
    }
}