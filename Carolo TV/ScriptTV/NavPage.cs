using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaroloTV.Models
{
    public class NavPage
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
        public int MinPage { get; set; }
        public int MaxPage { get; set; }

        public NavPage(int currentpage, int totalpage, int nbrButton=10)
        {
            TotalPage = totalpage <= 0 ? 0 :  totalpage-1;
            CurrentPage = currentpage > TotalPage ? TotalPage : currentpage;
            nbrButton /= 2;
            Before = CurrentPage - 1 < 0 ? 0 : CurrentPage - 1;
            After = CurrentPage + 1 > TotalPage ? TotalPage : CurrentPage + 1;
            MinPage = CurrentPage - nbrButton < 0 ? 0 : CurrentPage - nbrButton;
            MaxPage = MinPage + 10 > TotalPage ? TotalPage : MinPage + 10;
        }
    }
}
