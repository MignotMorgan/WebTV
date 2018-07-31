//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.SqlServer.Server;
using System.Data.Entity;
using System.Data.Entity.Utilities;

using CaroloTV.Models;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace CaroloTV.Services
{
    public class WebTVDbContext : DbContext, IService
    {
        public string Name { get; private set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Proprietary> Proprietarys { get; set; }
        public DbSet<Hourly> Hourlys { get; set; }

        public WebTVDbContext() { Name = "Divers"; }
        public WebTVDbContext(string name)
    : base(nameOrConnectionString: ConnectionString(name))
            //: base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebTV_" + name + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true;")
        {
            Name = name;
            //InitChannel();
        }

        private static string ConnectionString(string name)
        {
            //SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            //sqlBuilder.DataSource = "(localdb)\\MSSQLLocalDB";
            //sqlBuilder.InitialCatalog = "WebTV_" + name;
            //sqlBuilder.PersistSecurityInfo = true;
            //sqlBuilder.IntegratedSecurity = true;
            //sqlBuilder.MultipleActiveResultSets = true;

            //EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            //entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            //entityBuilder.Metadata = "res://*/";
            //entityBuilder.Provider = "System.Data.SqlClient";

            //return entityBuilder.ToString();


            //Fichier
            //return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C: \Users\Shariz\documents\visual studio 2017\Projects\Carolo TV\Carolo TV\App_Data\DBInformavie.mdf';Integrated Security=True";
            //SQL EXPRESS
            return @"Data Source=R2D2\SQLEXPRESS;Initial Catalog=CaroloTV_" + name + ";Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true;";
            //SQL Local
            //return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CaroloTV_" + name + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true;";





            //string request = @"Data Source = R2D2\SQLEXPRESS;";
            //request += "Initial Catalog = WebTVServer_" + name + ";";
            //request += "Integrated Security = True;";
            //request += "Pooling = False;";
            //request += "MultipleActiveResultSets = true;";
            //return request;

            //return "Data Source = (localdb); Initial Catalog = CaroloTV_Informavie; Integrated Security = True; providerName='System.Data.SqlClient'";

        }
        private void InitChannel()
        {
            //Database.CreateIfNotExists();

            //Proprietary proprietary = new Proprietary
            //{
            //    Name = "Proprietary"
            //};
            //Add(proprietary);
            //Media media =new Media
            //{
            //    Name = "Media",
            //    Src = "8CmvgFizLEs",
            //    Duration = 50,
            //    Description = "Description",
            //    ProprietaryID = proprietary.ProprietaryID,
            //    Proprietary = proprietary
            //};
            //Add(media);


        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string request = @"Data Source = R2D2\SQLEXPRESS;";
        //    request += "Initial Catalog = WebTVServer_" + Name + ";";
        //    request += "Integrated Security = True;";
        //    request += "Pooling = False;";
        //    request += "MultipleActiveResultSets = true;";

        //    //request = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = WebTV_Test; Integrated Security = True; Pooling=False";

        //    request = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebTV_" + Name + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true;";


        //    optionsBuilder.UseSqlServer(request, b => b.UseRowNumberForPaging());//.UseRowNumberForPaging();

        //    //optionsBuilder.UseSqlServer(@"Data Source = R2D2\SQLEXPRESS; Initial Catalog = WebTVServer" +Name+"; Integrated Security = True; Pooling = False", b => b.UseRowNumberForPaging());//.UseRowNumberForPaging();
        //    //optionsBuilder.UseSqlServer(@"Data Source = C:\Donée\employés.mdb; Integrated Security = True;", b => b.UseRowNumberForPaging());//.UseRowNumberForPaging();

        //    //Database.EnsureCreated();
        //}

        public News FindNews() { return null; }
        public void Save(News news) { }
        private Hourly NullableHourly { get { return null; } }
        public Proprietary NullableProprietary { get { return null; } }
        public Media NullableMedia { get { return null; } }

        public Hourly Current
        {
            get
            {
                try
                {
                    DateTime now = DateTime.Now;
                    return Hourlys
                        .Include(h => h.Media)
                        .Where(h => h.Date <= now)
                        .Where(h => h.Date + new TimeSpan(0, 0, h.Media.Duration) > now)
                        .First();
                }
                catch (Exception e)
                {
                    return NullableHourly;
                }
            }
        }
        public Hourly Next
        {
            get
            {
                Hourly hourlyCurrent = Current;
                if (hourlyCurrent != null)
                {
                    DateTime now = DateTime.Now + new TimeSpan(0, 0, hourlyCurrent.Media.Duration);
                    try
                    {
                        return Hourlys
                            .Include(h => h.Media)
                            .Where(h => h.Date <= now)
                            .Where(h => h.Date + new TimeSpan(0, 0, h.Media.Duration) > now)
                            .First();
                    }
                    catch (Exception e)
                    {
                        return NullableHourly;
                    }
                }
                return NullableHourly;
            }
        }
        private Hourly First
        {
            get
            {
                if (Hourlys.Count() > 0)
                    return Hourlys
                        .Include(h => h.Media)
                        .First();
                else
                    return NullableHourly;
            }
        }
        public Hourly Last
        {
            get
            {
                if (Hourlys.Count() > 0)
                    return Hourlys
                        .Last();
                else
                    return NullableHourly;
            }
        }
        public void NewHourly(string mname, string pname, string hname, bool hourlycurrent)
        { }
        public IEnumerable<Hourly> FindHourlyAll()
        {
            lock (Hourlys)
            {
                return Hourlys
                    .Include(h => h.Media)
                    .ToList();
            }
        }
        public IEnumerable<string> FindHourlyList()
        {
            return null;
        }

        public IEnumerable<Hourly> FindHourly(string name) { return null; }
        public void CreateHourly(string name) { }
        public IEnumerable<Media> FindMediaAll()
        {
            return Medias
                .ToList();
        }
        public ICollection<Proprietary> FindProprietaryAll()
        {
            return Proprietarys
                .ToList();
        }

        private Hourly FindHourlyByID(int id)
        {
            return Hourlys
                .Find(id);
        }

        public Media FindMediaByID(int id)
        {
            return Medias
                .Find(id);
        }
        public Proprietary FindProprietaryByID(int id)
        {
            return Proprietarys
                .Find(id);
        }
        public IEnumerable<Media> FindMediaByProprietary(string proprietaryname)
        {
            return Medias
                .Include(m => m.Proprietary)
                .Where(m => m.Proprietary.Name == proprietaryname)
                .ToList();
        }
        public IEnumerable<Proprietary> FindProprietaryByKeyword(string name)
        {
            return Proprietarys.Where(p => p.Name.Contains(name)).ToList();
        }
        public Proprietary FindProprietaryByName(string name)
        {
            try
            {
                return Proprietarys
                    .Where(h => h.Name == name)
                    .First();
            }
            catch (Exception e)
            {
                return NullableProprietary;
            }
        }
        public Media FindMediaByName(string name, string proprietaryname)
        {
            try
            {
                return Medias
                    .Where(h => h.Name == name)
                    .First();
            }
            catch (Exception e)
            {
                return NullableMedia;
            }
        }

        public void Add(Hourly hourly, string name)
        {
            Hourlys.Add(hourly);
            SaveChanges();
        }
        public void Add(Media media)
        {
            Medias.Add(media);
            SaveChanges();
        }
        public void Add(Proprietary proprietary)
        {
            Proprietarys.Add(proprietary);
            SaveChanges();
        }

        public void Delete(Hourly hourly)
        {
            //Hourlys.Remove(hourly);
            //SaveChanges();

            //Hourly h = First;
            //Reorganize(h.Date,"");
        }
        public void Delete(Media media)
        {
            Medias.Remove(media);
            SaveChanges();
        }
        public void Delete(Proprietary proprietary)
        {
            Proprietarys.Remove(proprietary);
            SaveChanges();
        }
        public void Update(Media media)
        {
            Entry<Media>(media).State = EntityState.Modified;
            //Medias.Update(media);
            SaveChanges();
        }
        public void Update(Proprietary proprietary)
        {
            Entry<Proprietary>(proprietary).State = EntityState.Modified;
            //Proprietarys.Update(proprietary);
            SaveChanges();
        }
        public void Save(Media media)
        {
            if (!Medias.Contains(media))
                Add(media);
            SaveChanges();
        }

        public void Save(Proprietary proprietary)
        {
            if (!Proprietarys.Contains(proprietary))
                Add(proprietary);
            SaveChanges();
        }

        public void DeleteHourly(string guid, string name, bool hourlycurrent)
        {
            //Hourly hourly = FindHourlyByID(guid);
            //if (hourly != null)
            //    Delete(hourly);
        }
        public void Reorganize(DateTime firstdate, string name, bool hourlycurrent)
        {
            lock (this)
            {
                Hourly last = null;
                foreach (Hourly h in Hourlys.Include(h => h.Media).ToList())
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
                SaveChanges();
            }
        }
        public void PlaceWebTV(string name)
        {

        }
        public void AjouteWebTV(string name)
        {

        }




    }
}