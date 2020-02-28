using OnlineBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBook.Controllers
{        
    public class AdminController : Controller
    {
        private static BookOnlineEntities entities = new BookOnlineEntities();
        // GET: Admin
        public ActionResult Index()
        {            
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string name, string password)
        {
            if(name == "Admin" && password == "0123429320")
            {
                Session["Logged"] = true;
                return RedirectToAction(nameof(Logged));
            }
            else
            {
                return RedirectToAction(nameof(LoginErr));
            }
        }

        public ActionResult Logged()
        {
            try
            {
                bool isLogged = (bool)Session["Logged"];
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }            
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            try
            {
                if(ModelState.IsValid)
                {
                    bool isLogged = (bool)Session["Logged"];
                    return View();
                }
                else
                {
                    return RedirectToAction(nameof(Logged));
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            entities.Categories.Add(category);
            entities.SaveChanges();
            return RedirectToAction(nameof(Logged));
        }
        [HttpGet]
        public ActionResult CreateBook()
        {
            return View(entities.Categories.ToList());
        }

        [HttpPost]
        public ActionResult CreateBook(Book book, HttpPostedFileBase pdf)
        {
            pdf.SaveAs(Server.MapPath("~/files/" + pdf.FileName));
            book.book_url = Server.MapPath("~/files/" + pdf.FileName);
            entities.Books.Add(book);

            entities.SaveChanges();
            return RedirectToAction(nameof(Logged));
        }

        public ActionResult Download(string file)
        {            
            return File(file, "Application/pdf");
        }

        public ActionResult Categories()
        {
            return View(entities.Categories.ToList());
        }

        public ActionResult DeleteCategory()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            var query = from cate in entities.Categories where cate.id == id select cate;
            Category firstCate = query.First();

            entities.Categories.Remove(firstCate);
            entities.SaveChanges();

            return RedirectToAction(nameof(Logged));
        }

        public ActionResult Books(List<Book> Books = null)
        {            
            return View(Books??entities.Books.ToList());
        }

        public ActionResult SearchBooks(string book_name)
        {
            var books = from b in entities.Books where b.book_name == book_name select b;
            List<Book> foundBooks = books.ToList();
            return RedirectToAction(nameof(Books), foundBooks);
        }

        public ActionResult DeleteBook()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            var query = from book in entities.Books where book.id == id select book;
            var fbook = query.First();
            entities.Books.Remove(fbook);
            entities.SaveChanges();

            return RedirectToAction(nameof(Books));
        }

        public ActionResult LoginErr()
        {
            return View();
        }        
    }
}