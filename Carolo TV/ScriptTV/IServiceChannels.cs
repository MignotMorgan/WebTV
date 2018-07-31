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
    public interface IServiceChannels
    {
        IEnumerable<News> FindAllNews();
        void Save(News news);
    }
}