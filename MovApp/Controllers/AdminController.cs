using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovApp.Models;
using System.Linq.Dynamic;

namespace MovApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private movieEntities db = new movieEntities();

        //
        // GET: /Admin/

        public ActionResult Index(PageModel pageModel)
        {



            var movie = db.Movies.Include(m => m.Genre);

            if (!String.IsNullOrEmpty(pageModel.Search))
            {
                movie = movie.Where(s => s.Title.Contains(pageModel.Search) || s.Description.Contains(pageModel.Search) || s.Genre.Name.Contains(pageModel.Search));
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
            movie = movie.OrderBy(orderParam);

            return View(new MovieListModel
            {
                MovieList = movie.ToList(),
                Page = pageModel
            });

        }

        //
        // GET: /Admin/Details/5

        public ActionResult Details(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        //
        // GET: /Admin/Create
        
        public ActionResult Create()
        {
            ViewBag.Genre_Id = new SelectList(db.Genres, "Id", "Name");
            return View();
        }

        //
        // POST: /Admin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Genre_Id = new SelectList(db.Genres, "Id", "Name", movie.Genre_Id);
            return View(movie);
        }

        //
        // GET: /Admin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.Genre_Id = new SelectList(db.Genres, "Id", "Name", movie.Genre_Id);
            return View(movie);
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Genre_Id = new SelectList(db.Genres, "Id", "Name", movie.Genre_Id);
            return View(movie);
        }

        //
        // GET: /Admin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}