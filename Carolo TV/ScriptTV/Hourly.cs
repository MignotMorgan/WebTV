using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaroloTV.Models
{
    public class Hourly
    {
        public string Guid { get; set; }
        [Range(1,9999)]
        public int Year { get; set; }
        [Range(1,12)]
        public int Month { get; set; }
        [Range(1,31)]
        public int Day { get; set; }
        [Range(0,23)]
        public int Hour { get; set; }
        [Range(0,59)]
        public int Minute { get; set; }
        [Range(0,59)]
        public int Second { get; set; }
        public string MediaName { get; set; }
        public string ProprietaryName { get; set; }
        public int MediaID { get; set; }
        public virtual Media Media { get; set; }
        [NotMapped]
        public DateTime Date
        {
            get { return new DateTime(Year, Month, Day, Hour, Minute, Second); }
            set
            {
                Year = value.Year;
                Month = value.Month;
                Day = value.Day;
                Hour = value.Hour;
                Minute = value.Minute;
                Second = value.Second;
            }
        }
    }
}
