using MovApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Linq.Dynamic;
using PagedList;
using System.Data.Entity.SqlServer;

namespace MovApp.Controllers
{
    public class HomeController : Controller
    {
        private movieEntities db = new movieEntities();
        const int pageSize = 3;

        [HttpGet]
        public ActionResult Movies(int page = 1, int sortBy = 1, bool isAsc = true, string search = null){
            //if (!string.IsNullOrWhiteSpace(search)) { search = search.ToString(); }

            IEnumerable<Movie> movies = db.Movies.Include(p => p.Genre).Where(
           p => search == null
           || p.Title.Contains(search)
           || p.Genre.Name.Contains(search)
           || p.Description.Contains(search)
           || SqlFunctions.StringConvert((double)p.Year).Contains(search));
            
            #region Sorting
            switch (sortBy)
            {
                case 1:
                    movies = isAsc ? movies.OrderBy(p => p.Title) : movies.OrderByDescending(p => p.Title);
                    break;

                case 2:
                    movies = isAsc ? movies.OrderBy(p => p.Year) : movies.OrderByDescending(p => p.Year);
                    break;
                
                default:
                    movies = isAsc ? movies.OrderBy(p => p.Rate) : movies.OrderByDescending(p => p.Rate);
                    break;
            }
            #endregion

            ViewBag.TotalPages = (int)Math.Ceiling((double)movies.Count() / pageSize);

            movies = movies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
           
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
           
            ViewBag.SortBy = sortBy;
            ViewBag.IsAsc = isAsc;
            ViewBag.Search = search;

            return View(movies);
        }

        public ActionResult Details(int id)
        {
            var movieDetails = (from Movie in db.Movies
                                where Movie.Id == id
                                select Movie).First();

            return View(movieDetails);
        }
        [HttpGet]
        public ActionResult Index(int? page)
        {
            var movie = db.Movies.Include(m => m.Genre);



            /*
            if (!String.IsNullOrEmpty(pageModel.Search))
            {
                movie = movie.Where(s => s.Title.Contains(pageModel.Search) 
             * || s.Description.Contains(pageModel.Search) 
             * || s.Genre.Name.Contains(pageModel.Search));
            }

            string orderParam = "Id descending";
            if (!string.IsNullOrWhiteSpace(pageModel.SortedColumn))
            {
                orderParam = pageModel.SortedColumn;
                if (pageModel.isDescending)
                {
                    orderParam += " descending";
                }
            }

            movie = movie.OrderBy(orderParam);*/

            int pageNumber = page ?? 1;
            int pageSize = 3;


            return View(movie.OrderBy("Title desc").ToPagedList(pageNumber, pageSize));

        }

    }
}
