using NoSql.DAL;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using System.Web.WebPages;

namespace NoSql.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILibrarian _librarian;

        public HomeController(ILibrarian librarian)
        {
            _librarian = librarian;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Users = "admin")]
        public void UpdateDb(string flag)
        {
            try
            {
                _librarian.UpdateDbLiblirary(flag == "clean");
            }
            catch (Exception )
            {
                Response.SubStatusCode = (int) HttpStatusCode.InternalServerError;
            }
        }

        [HttpPost]
        public JsonResult GetBooks(string comm)
        {
            return  new JsonResult(){Data = _librarian.GetBooks()};
        }

        [HttpGet]
        public void Validate(string number)
        {
            if (!(number != null && !number.IsEmpty() && number.AsInt() <= _librarian.GetBooks().Count()))
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
            }
        }

        [HttpGet]
        public ActionResult GetBook(int? id)
        {
            var book = _librarian.GetBooks().ElementAt(id.Value);
            byte[] fileBytes = System.IO.File.ReadAllBytes(book.Path);
            string fileName = book.Name + book.Extension;
            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}