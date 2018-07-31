using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaroloTV.Models;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;

namespace CaroloTV.Services
{
    public class WebTVXML : IService
    {
        public string Name { get; private set; }
        private Encoding Encoding = Encoding.UTF8;
        private string Addresse_CaroloTV { get; set; }

       public WebTVXML(string name)
        {
            Name = name;
            Addresse_CaroloTV = @"C:\inetpub\SandBox\CaroloTV\" + Name + @"\";
            DirectoryInfo directory = new DirectoryInfo(Addresse_CaroloTV);
            if (!directory.Exists)
                directory.Create();
        }

        private Hourly NullableHourly { get { return null; } }
        public Proprietary NullableProprietary { get { return new Proprietary { Name="Anonyme" }; } }
        private Media NullableMedia { get { return null; } }

        //private Hourly NullableHourly { get { return new Hourly { Media = NullableMedia, Date = DateTime.Now }; } }
        //private Proprietary NullableProprietary { get { return new Proprietary { Name = "Anonyme" }; } }
        //private Media NullableMedia { get { return new Media { Name = "Mire", Src = "yaqe1qesQ8c", Duration = 358, Proprietary = NullableProprietary, Description = "WebTV en attente!" }; } }

        public Hourly Current
        {
            get
            {
                List<Hourly> hourlys = XmlLoadHourlys().ToList<Hourly>();
                DateTime now = DateTime.Now;
                foreach (Hourly h in hourlys)
                {
                    if (h.Date <= now && h.Date + new TimeSpan(0, 0, h.Media.Duration) > now)
                        return h;
                }
                return NullableHourly;
            }
        }

        public Hourly Next
        {
            get
            {
                Hourly hourlyCurrent = Current;
                if (hourlyCurrent != null)
                {
                    List<Hourly> hourlys = XmlLoadHourlys().ToList<Hourly>();
                    DateTime now = DateTime.Now + new TimeSpan(0, 0, hourlyCurrent.Media.Duration);
                    foreach (Hourly h in hourlys)
                    {
                        if (h.Date <= now && h.Date + new TimeSpan(0, 0, h.Media.Duration) > now)
                            return h;
                    }
                }
                return NullableHourly;
            }
        }

        public Hourly Last
        {
            get
            {
                List<Hourly> hourlys = XmlLoadHourlys().ToList<Hourly>();
                if (hourlys.Count() > 0)
                    return hourlys.Last();
                else
                    return NullableHourly;
            }
        }
        //***************************************************************************************************************
        public void NewHourly(string mname, string pname, string hname, bool hourlycurrent)
        {
            string path = hourlycurrent ? PathHourly() : PathHourlyList(hname);
            List<Hourly> hourlys = XmlLoadHourlys(path).ToList();

            DateTime lastTime;
            if(hourlys.Count() > 0)
            {
                Hourly last = hourlys.Last();
                lastTime = last.Date + new TimeSpan(0, 0, last.Media.Duration);
            }
            else
            {
                lastTime = DateTime.Now;
            }
            Hourly hourly = new Hourly
            {
                Date = lastTime,
                MediaName = mname,
                ProprietaryName = pname,
                Guid = Guid.NewGuid().ToString()
            };

            hourlys.Add(hourly);
            XmlSaveHourlys(hourlys, path);
        }
        //public void Add(Hourly hourly, string name)
        //{
        //    string path = PathHourlyList(name);
        //    List<Hourly> hourlys = XmlLoadHourlys(path).ToList();
        //    hourlys.Add(hourly);
        //    XmlSaveHourlys(hourlys, path);
        //}

        public void Delete(Media media)
        {
            string path = PathMedia(media.Name, media.Proprietary.Name);
            if (File.Exists(path))
                File.Delete(path);
        }

        public void Delete(Proprietary proprietary)
        {
            string path = PathProprietary(proprietary.Name);
            if (File.Exists(path))
                File.Delete(path);
            DirectoryInfo directory = new DirectoryInfo(PathProprietaryDirectory(proprietary.Name));
            if (directory.Exists)
            {
                foreach (FileInfo f in directory.GetFiles())
                {
                    f.Delete();
                    //f.MoveTo(PathProprietaryDirectory(NullableProprietary.Name));
                }
                directory.Delete();
            }
        }
        public void DeleteHourly(string guid, string name, bool hourlycurrent)
        {
            string path = hourlycurrent ? PathHourly() : PathHourlyList(name);
            List<Hourly> hourlys = XmlLoadHourlys(path).ToList();
            //List<Hourly> hourlys = XmlLoadHourlys().ToList<Hourly>();
            Hourly hourly = null;
            foreach(Hourly h in hourlys)
                if(h.Guid == guid)
                {
                    hourly = h;
                    break;
                }
            hourlys.Remove(hourly);
            XmlSaveHourlys(hourlys,path);
        }

        public IEnumerable<Hourly> FindHourlyAll()
        {
            return XmlLoadHourlys();
        }
        public IEnumerable<string> FindHourlyList()
        {
            string path = PathDirectoryHourly();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            List<string> list = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
                foreach (FileInfo f in directory.GetFiles())
                {
                    list.Add(Path.GetFileNameWithoutExtension(f.FullName));
                }
            return list;
        }
        public IEnumerable<Hourly> FindHourly(string name)
        {
            string path = PathHourlyList(name);
            if (!File.Exists(path))
                return new List<Hourly>();
            return XmlLoadHourlys(path).ToList();
        }
        public void CreateHourly(string name)
        {
            string path = PathHourlyList(name);
            List<Hourly> list = new List<Hourly>();
            XmlSaveHourlys(list, path);
        }



        public IEnumerable<Media> FindMediaAll()
        {
            string path = Addresse_CaroloTV + @"Medias\";
            List<Media> list = new List<Media>();
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
                foreach (DirectoryInfo d in directory.GetDirectories())
                    foreach (FileInfo f in d.GetFiles())
                    {
                        if (f.Name == "Proprietary.xml") continue;
                        Media m = XmlLoadMedia(f.FullName);
                        list.Add(m);
                    }
            return list;
        }

        public ICollection<Proprietary> FindProprietaryAll()
        {
            string path = Addresse_CaroloTV + @"Medias\";
            List<Proprietary> list = new List<Proprietary>();
            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
                directory.Create();
            foreach (DirectoryInfo d in directory.GetDirectories())
            {
                string filename = d.FullName + "\\Proprietary.xml";
                if (File.Exists(filename))
                {
                    Proprietary p = FindProprietaryByName(d.Name);
                    list.Add(p);
                }
            }
            return list;
        }

        public IEnumerable<Media> FindMediaByProprietary(string proprietaryname)
        {
            string path = PathProprietaryDirectory(proprietaryname);
            List<Media> list = new List<Media>();
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
                foreach (FileInfo f in directory.GetFiles())
                {
                    if (f.Name == "Proprietary.xml") continue;
                    Media m = XmlLoadMedia(f.FullName);
                    list.Add(m);
                }
            return list;
        }
        public News FindNews()
        {
            return XmlLoadNews(PathNews());
        }

        public Proprietary FindProprietaryByName(string name)
        {
            return XmlLoadProprietary(name);
        }
        public Media FindMediaByName(string name, string proprietaryname)
        {
            return XmlLoadMedia(name, proprietaryname);
        }

        public void Save(Media media)
        {
            string path = PathMedia(media.Name, media.ProprietaryName);// PathProprietaryDirectory(media.ProprietaryName);
            XmlSaveMedia(media, path);
        }

        public void Save(Proprietary proprietary)
        {
            XmlSaveProprietary(proprietary);
        }
        public void Save(News news)
        {
            XmlSaveNews(news, PathNews());
        }
        public void Reorganize(DateTime firstdate, string name, bool hourlycurrent)
        {
            string path = hourlycurrent ? PathHourly() : PathHourlyList(name);
            List<Hourly> hourlys = XmlLoadHourlys(path).ToList();

            Hourly last = null;

            foreach (Hourly h in hourlys)
            {
                if (last == null)
                {
                    h.Date = firstdate;
                    last = h;
                }
                else
                {
                    h.Date = last.Date + new TimeSpan(0, 0, last.Media.Duration);
                    last = h;
                }
            }

            XmlSaveHourlys(hourlys, path);
        }
        public void PlaceWebTV(string name)
        {
            string path = PathHourlyList(name);
            List<Hourly> hourlys = XmlLoadHourlys(path).ToList();

            XmlSaveHourlys(hourlys, PathHourly());
        }
        public void AjouteWebTV(string name)
        {
            string path = PathHourlyList(name);
            List<Hourly> hourlys = XmlLoadHourlys(path).ToList();
            
            List<Hourly> hourlysWebTV = XmlLoadHourlys(PathHourly()).ToList();

            foreach (Hourly h in hourlys)
                hourlysWebTV.Add(h);
                
            XmlSaveHourlys(hourlysWebTV, PathHourly());
        }




        private string PathProprietaryDirectory(string proprietaryname)
        {
            return Addresse_CaroloTV +@"Medias\" + proprietaryname + @"\";
        }
        private string PathProprietary(string proprietaryname)
        {
            return PathProprietaryDirectory(proprietaryname) + "Proprietary.xml";
        }
        private string PathMedia(string medianame, string proprietaryname)
        {
            return PathProprietaryDirectory(proprietaryname) + medianame + ".xml";
        }
        private string PathNews()
        {
            return Addresse_CaroloTV + "News.xml";
        }
        private string PathHourly()
        {
            return Addresse_CaroloTV + "Hourlys.xml";
        }
        private string PathDirectoryHourly()
        {
            return Addresse_CaroloTV + @"Hourlys\";
        }
        private string PathHourlyList(string name)
        {
            return PathDirectoryHourly() + name + ".xml"; ;
        }

        private void XmlSaveProprietary(Proprietary proprietary)
        {
            string pathDirectory = PathProprietaryDirectory(proprietary.Name);
            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);
            string path = PathProprietary(proprietary.Name);

            XmlTextWriter writer = new XmlTextWriter(path, Encoding);

            writer.WriteStartDocument(true);
            writer.WriteStartElement("Proprietary");
            
            writer.WriteElementString("Name", proprietary.Name);
            writer.WriteElementString("ImageUrl", proprietary.ImageUrl);
            writer.WriteElementString("BackGroundUrl", proprietary.BackGroundUrl);
            writer.WriteElementString("Description", proprietary.Description);
            writer.WriteElementString("WebSite", proprietary.WebSite);
            writer.WriteElementString("WordPress", proprietary.WordPress);
            writer.WriteElementString("Youtube", proprietary.Youtube);
            writer.WriteElementString("Facebook", proprietary.Facebook);
            writer.WriteElementString("Twitter", proprietary.Twitter);
            writer.WriteElementString("Google", proprietary.Google);
            writer.WriteElementString("Dailymotion", proprietary.Dailymotion);
            writer.WriteElementString("Vimeo", proprietary.Vimeo);
            writer.WriteElementString("Email", proprietary.Email);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
        private Proprietary XmlLoadProprietary(string name)
        {
            string path = PathProprietary(name);
            if (!File.Exists(path))
                return NullableProprietary;
            XmlReader reader = XmlReader.Create(path);

            Proprietary proprietary = new Proprietary();

            reader.Read();
            if (reader.NodeType == XmlNodeType.XmlDeclaration) reader.Read();
            if (reader.IsStartElement()) reader.Read();
            
            proprietary.Name = reader.ReadElementString();
            proprietary.ImageUrl = reader.ReadElementString();
            proprietary.BackGroundUrl = reader.ReadElementString();
            proprietary.Description = reader.ReadElementString();
            proprietary.WebSite = reader.ReadElementString();
            proprietary.Youtube = reader.ReadElementString();
            proprietary.Facebook = reader.ReadElementString();
            proprietary.Twitter = reader.ReadElementString();
            proprietary.Google = reader.ReadElementString();
            proprietary.Dailymotion = reader.ReadElementString();
            proprietary.Vimeo = reader.ReadElementString();
            proprietary.Email = reader.ReadElementString();

            reader.Close();

            return proprietary;
        }
        private void XmlSaveMedia(Media media)
        {
            string pathDirectory = PathProprietaryDirectory(media.ProprietaryName);
            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);
            string path = PathMedia(media.Name, media.ProprietaryName);
            XmlSaveMedia(media, path);
        }
        private void XmlSaveMedia(Media media, string path)
        {
            string pathDirectory = PathProprietaryDirectory(media.ProprietaryName);
            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);
            XmlTextWriter writer = new XmlTextWriter(path, Encoding);

            writer.WriteStartDocument(true);
            writer.WriteStartElement("Media");
            
            writer.WriteElementString("Name", media.Name);
            writer.WriteElementString("Src", media.Src);
            writer.WriteElementString("VideoPlayer", ((int)media.VideoPlayer).ToString());
            writer.WriteElementString("Description", media.Description);
            writer.WriteElementString("StartSeconds", media.StartSeconds.ToString());
            writer.WriteElementString("Duration", media.Duration.ToString());
            writer.WriteElementString("ProprietaryName", media.ProprietaryName);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        private Media XmlLoadMedia(string name, string proprietaryname)
        {
            string path = PathMedia(name, proprietaryname);
            return XmlLoadMedia(path);
        }
        private Media XmlLoadMedia(string path)
        {
            if (!File.Exists(path))
                return NullableMedia;
            XmlReader reader = XmlReader.Create(path);

            Media media = new Media();

            reader.Read();
            if (reader.NodeType == XmlNodeType.XmlDeclaration) reader.Read();
            if (reader.IsStartElement()) reader.Read();
            
            media.Name = reader.ReadElementContentAsString();
            media.Src = reader.ReadElementContentAsString();
            media.VideoPlayer = (VideoPlayer)reader.ReadElementContentAsInt();
            media.Description = reader.ReadElementContentAsString();
            media.StartSeconds = reader.ReadElementContentAsInt();
            media.Duration = reader.ReadElementContentAsInt();
            media.ProprietaryName = reader.ReadElementContentAsString();

            reader.Close();

            media.Proprietary = XmlLoadProprietary(media.ProprietaryName);
            return media;
        }


        private void XmlSaveHourlys(IEnumerable<Hourly> hourlys)
        {
            XmlSaveHourlys(hourlys, PathHourly());
        }
        private void XmlSaveHourlys(IEnumerable<Hourly> hourlys, string path)
        {
            XmlTextWriter writer = new XmlTextWriter(path, Encoding);

            writer.WriteStartDocument(true);
            writer.WriteStartElement("Hourlys");

            writer.WriteElementString("Count", hourlys.Count().ToString());
            foreach (Hourly h in hourlys)
            {
                writer.WriteStartElement("Hourly");

                writer.WriteElementString("Guid", h.Guid);
                writer.WriteElementString("Year", h.Year.ToString());
                writer.WriteElementString("Month", h.Month.ToString());
                writer.WriteElementString("Day", h.Day.ToString());
                writer.WriteElementString("Hour", h.Hour.ToString());
                writer.WriteElementString("Minute", h.Minute.ToString());
                writer.WriteElementString("Second", h.Second.ToString());
                writer.WriteElementString("MediaName", h.MediaName);
                writer.WriteElementString("ProprietaryName", h.ProprietaryName);
                writer.WriteElementString("MediaID", h.MediaID.ToString());

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
        private IEnumerable<Hourly> XmlLoadHourlys()
        {
            string path = PathHourly();
            return XmlLoadHourlys(path);
        }
        private IEnumerable<Hourly> XmlLoadHourlys(string path)
        {
            List<Hourly> hourlys = new List<Hourly>();
            if (!File.Exists(path))
                return hourlys;

            XmlReader reader = XmlReader.Create(path);

            reader.Read();
            if (reader.NodeType == XmlNodeType.XmlDeclaration) reader.Read();
            if (reader.IsStartElement()) reader.Read();

            int Count = reader.ReadElementContentAsInt();

            for (int i = 0; i < Count; i++)
            {
                Hourly hourly = new Hourly();
                reader.ReadStartElement();
                hourly.Guid = reader.ReadElementContentAsString();
                hourly.Year = reader.ReadElementContentAsInt();
                hourly.Month = reader.ReadElementContentAsInt();
                hourly.Day = reader.ReadElementContentAsInt();
                hourly.Hour = reader.ReadElementContentAsInt();
                hourly.Minute = reader.ReadElementContentAsInt();
                hourly.Second = reader.ReadElementContentAsInt();
                hourly.MediaName = reader.ReadElementContentAsString();
                hourly.ProprietaryName = reader.ReadElementContentAsString();
                hourly.MediaID = reader.ReadElementContentAsInt();
                reader.ReadEndElement();

                hourly.Media = XmlLoadMedia(hourly.MediaName, hourly.ProprietaryName);
                if (hourly.Media != NullableMedia)
                    hourlys.Add(hourly);
            }
            reader.Close();
            return hourlys;
        }


        private void XmlSaveNews(News news, string path)
        {
            string pathDirectory = PathNews();
            XmlTextWriter writer = new XmlTextWriter(path, Encoding);

            writer.WriteStartDocument(true);
            writer.WriteStartElement("Media");

            writer.WriteElementString("Channel", news.Channel);
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
                news.Hourly = NullableHourly;
                return news;
            }
            XmlReader reader = XmlReader.Create(path);

            reader.Read();
            if (reader.NodeType == XmlNodeType.XmlDeclaration) reader.Read();
            if (reader.IsStartElement()) reader.Read();

            news.Channel = reader.ReadElementContentAsString();
            news.HourlyGuid = reader.ReadElementContentAsString();
            news.StartSeconds = reader.ReadElementContentAsInt();
            news.Duration = reader.ReadElementContentAsInt();
            news.Image = reader.ReadElementContentAsString();
            news.Title = reader.ReadElementContentAsString();
            news.Resume = reader.ReadElementContentAsString();
            news.Description = reader.ReadElementContentAsString();

            reader.Close();

            List<Hourly> hourlys = FindHourlyAll().ToList();
            foreach (Hourly h in hourlys)
                if (h.Guid == news.HourlyGuid)
                    news.Hourly = h;

            return news;
        }



    }
}
