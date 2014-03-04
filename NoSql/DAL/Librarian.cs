using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using MongoRepository;
using NoSql.Controllers;
using NoSql.Models;
using NoSql.Models.DbModels;

namespace NoSql.DAL
{
    public class Librarian : ILibrarian
    {
        public IRepository<Book> _repo;

        public Librarian(IRepository<Book> repository )
        {
            _repo = repository;
        }

        public IEnumerable<Book> GetBooks()
        {
            return new List<Book>();
        }

        public void UpdateLiblirary()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@"D:\Dropbox\Library\Прочитанные");
            var s = directoryInfo.GetFiles("*.*",SearchOption.AllDirectories).Where(x => x.Extension != ".exe");
        }
    }

    public interface ILibrarian
    {
        IEnumerable<Book> GetBooks();
        void UpdateLiblirary();
    }
}