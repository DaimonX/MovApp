using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovApp.Models
{
    public class PageModel
    {
        public string SortedColumn { get; set; }
        public bool isDescending { get; set; }

        public string Search { get; set; }

        public int PageSize = 2;
        public int? Pag { get; set; }
    }
    public class MovieListModel
    {
        public List<MovApp.Models.Movie> MovieList { get; set; }

        public PageModel Page { get; set; }

        
    }
}