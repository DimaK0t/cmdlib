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
using NoSql.Models.DbModels;

namespace NoSql.ControllersS
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
            return View();
        }

        [HttpPost]
        public JsonResult GetBooks(string comm)
        {
            _librarian.UpdateLiblirary();
            var result = new List<Book> {new Book() {Author = "Muracami", Name = "What I`m tolking about...", Path = @"C:\\dyd.fb2"},
                                        new Book() {Author = "Frai", Name = "Stranger"}};
            return  new JsonResult(){Data = result};
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