using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoRepository;
using System.Runtime.Serialization ;
using NoSql.DAL;
using NoSql.Models;

namespace NoSql.Controllers
{
    public class HomeController : Controller
    {
        private ILibrarian _librarian;

        public HomeController(ILibrarian librarian)
        {
            _librarian = librarian;
        }

        public ActionResult Index()
        {
            _librarian.GetBooks();
            return View();
        }

        [HttpPost]
        public JsonResult Index(string comm)
        {
            var result = new List<Book> {new Book() {Author = "Muracami", Name = "What I`m tolking about...", Path = @"C:\\dyd.fb2"}, new Book() {Author = "Frai", Name = "Stranger"}};
            var jsonResult = result.ToJson();
            var s =  new JsonResult(){Data = result};
            return s;
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

    public class Entry : Entity
    {
        public string Name { get; set; }
    }
}