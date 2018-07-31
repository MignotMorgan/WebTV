using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaroloTV.Models
{
    public class News
    {
        public string Channel { get; set; }
        public bool Active { get; set; }
        public string HourlyGuid { get; set; }
        [NotMapped]
        public Hourly Hourly { get; set; }

        public int StartSeconds { get; set; } = 0;
        public int Duration { get; set; }

        public string Image { get; set; }
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Description { get; set; }
    }
}