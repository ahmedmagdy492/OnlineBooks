using OnlineBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBook.Controllers
{
    public class HomeController : Controller
    {
        private static BookOnlineEntities entities = new BookOnlineEntities();
        // GET: Home
        public ActionResult Index()
        {               
            return View(entities.Books.ToList());
        }

        public ActionResult BooksCate()
        {
            int id = Convert.ToInt32(RouteData.Values["id"]);
            var cate = entities.Books.Where(book => book.cate_id == id);

            try
            {
                return View(cate.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        
    }
}