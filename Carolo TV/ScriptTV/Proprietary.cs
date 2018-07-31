using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaroloTV.Models
{
    public class Proprietary
    {
        [Required(ErrorMessage ="un nom est requis")]
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string BackGroundUrl { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        public string WordPress { get; set; }
        public string Youtube { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Google { get; set; }
        public string Dailymotion { get; set; }
        public string Vimeo { get; set; }
        public string Email { get; set; }
    }
}
