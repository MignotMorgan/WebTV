using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaroloTV.Models;

namespace CaroloTV.Services
{
    public interface IService
    {
        Hourly Current { get; }
        Hourly Next { get; }
        Hourly Last { get; }
        void NewHourly(string mname, string pname, string hname, bool hourlycurrent);
        //void Add(Hourly hourly, string name);
        void Delete(Media media);
        void Delete(Proprietary proprietary);
        void DeleteHourly(string guid, string name, bool hourlycurrent);
        IEnumerable<Hourly> FindHourlyAll();
        IEnumerable<string> FindHourlyList();
        IEnumerable<Hourly> FindHourly(string name);
        void CreateHourly(string name);
        IEnumerable<Media> FindMediaAll();
        ICollection<Proprietary> FindProprietaryAll();
        IEnumerable<Media> FindMediaByProprietary(string proprietaryname);
        Proprietary FindProprietaryByName(string name);
        Media FindMediaByName(string name, string proprietaryname);
        void Save(Media media);
        void Save(Proprietary proprietary);
        void Reorganize(DateTime firstdate, string name, bool hourlycurrent);
        void PlaceWebTV(string name);
        void AjouteWebTV(string name);
        News FindNews();
        void Save(News news);
    }
}
