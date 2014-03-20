using System.Collections.Generic;
using System.IO;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using MongoRepository;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using System.Web.WebPages;
using NoSql.Models.DbModels;

namespace NoSql.Controllers
{
    public class HomeController : Controller
    {
        public IRepository<Book> _repo ;

        public HomeController(IRepository<Book> repository)
        {

            _repo = repository;
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
                UpdateDbLiblirary(flag == "clean");
            }
            catch (Exception e)
            {
                Response.SubStatusCode = (int) HttpStatusCode.InternalServerError;
            }
        }

        [HttpPost]
        public JsonResult GetBooks(int pageNumber = 1)
        {
            var booksPerPage = 30;
            int.TryParse(WebConfigurationManager.AppSettings["booksPerPage"], out booksPerPage);
            var books = _repo.OrderBy(x => x.BookNumber).ToList();
            List<Book> booksForView;
            var pagesCount = books.Count() / booksPerPage + (books.Count() % booksPerPage > 0 ? 1 : 0);

            if (pageNumber > pagesCount)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
                return null;
            }

            if (pageNumber <= 0)
            {
                booksPerPage = books.Count;
                booksForView = books;
                pageNumber = 1;
                pagesCount = 1;
            }
            else
            {
                var lastPageAmount = books.Count - ((pageNumber - 1) * booksPerPage);
                booksForView = books.GetRange((pageNumber - 1) * booksPerPage, Math.Min(lastPageAmount, booksPerPage));
            }

            return new JsonResult() { Data = new {Books = booksForView, PagesCount = pagesCount, CurrentPage = pageNumber }};
        }

        [HttpGet]
        public void Validate(int number)
        {
            if (!_repo.Exists(x => x.BookNumber == number))
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }

        [HttpGet]
        public ActionResult GetBook(string bookNumber)
        {
            var book = _repo.First(x => x.BookNumber == int.Parse(bookNumber));
            byte[] fileBytes = System.IO.File.ReadAllBytes(book.Path);
            string fileName = string.Format("{0} - {1}{2}",book.Author, book.Name, book.Extension);
            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult About()
        {
            return PartialView();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void UpdateDbLiblirary(bool cleanBefore = false)
        {
            if (cleanBefore)
            {
                _repo.Collection.RemoveAll();
            }
            var libraryPath = WebConfigurationManager.AppSettings["libraryPath"];

            var directoryInfo = new DirectoryInfo(libraryPath);
            var files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).Where(x => x.Extension != ".exe").ToList();

            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var author = file.Name.Substring(0, file.Name.IndexOf('-')).Trim();
                var name = Path.GetFileNameWithoutExtension(file.Name.Substring(file.Name.IndexOf('-') + 1).Trim());
                var extenstion = file.Extension;
                var path = file.FullName;
                var book = new Book() { Author = author, Name = name, Extension = extenstion, Path = path, BookNumber = i };

                if (!_repo.Exists(x => x.Author == book.Author && x.Name == book.Name))
                {
                    _repo.Add(book);
                }
            }
        }
    }
}