using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using MongoRepository;
using NoSql.Controllers;
using NoSql.Models;

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
            var s = _repo;

             var entity = new Entry {Name = "Tom"};

            MongoRepository<Entry> repository =new MongoRepository<Entry>();
            MongoRepositoryManager<Entry> manager = new MongoRepositoryManager<Entry>();
            
            repository.Add(entity);
            var id = entity.Id; // Insert will set the Id if necessary (as it was in this example)

            return new List<Book>();
        }
    }

    public interface ILibrarian
    {
        IEnumerable<Book> GetBooks();
    }
}