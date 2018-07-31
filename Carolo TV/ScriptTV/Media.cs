using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaroloTV.Models
{
    public class Media
    {
        [Required(ErrorMessage = "un nom est requis")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "une source est requise")]
        public string Src { get; set; }
        [Required]
        public VideoPlayer VideoPlayer { get; set; } = VideoPlayer.Youtube;
        public int StartSeconds { get; set; } = 0;
        public int Duration { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public string ProprietaryName { get; set; } = "Anonyme";
        public virtual Proprietary Proprietary { get; set; }
        [NotMapped]
        public string Source
        {
            get
            {
                if (VideoPlayer == VideoPlayer.Youtube)
                    return @"//www.youtube.com/watch?v=" + Src;
                else if (VideoPlayer == VideoPlayer.Dailymotion)
                    return @"http://www.dailymotion.com/embed/video/" + Src;
                else return Src;
            }
        }
    }
}
