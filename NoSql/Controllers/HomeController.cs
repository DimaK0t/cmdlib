using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoRepository;
using System.Runtime.Serialization ;
using NoSql.DAL;

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
        public string UpdateDb(string flag)
        {
            try
            {
                _librarian.UpdateDbLiblirary(flag == "clean");
                return "succeed";
            }
            catch (Exception e)
            {
                return "failed" + e.Message;
            }
        }

        [HttpPost]
        public JsonResult GetBooks(string comm)
        {
            return  new JsonResult(){Data = _librarian.GetBooks()};
        }

        [HttpPost]
        public string GetBook(string number)
        {
            return number == null || number.IsEmpty() || number.AsInt() > _librarian.GetBooks().Count()
                ? "failed"
                : "succed";
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