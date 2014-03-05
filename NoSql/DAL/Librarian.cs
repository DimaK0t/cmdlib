using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoRepository;
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
            return _repo.ToList<Book>();
        }

        public void UpdateDbLiblirary(bool cleanBefore = false)
        {
            if (cleanBefore)
            {
                _repo.Collection.RemoveAll();
            }

            //TODO: Move it .config
            DirectoryInfo directoryInfo = new DirectoryInfo(@"D:\Dropbox\Library\Прочитанные");
            var files = directoryInfo.GetFiles("*.*",SearchOption.AllDirectories).Where(x => x.Extension != ".exe").ToList();

            foreach (var file in files)
            {
                var author = file.Name.Substring(0, file.Name.IndexOf('-')).Trim();
                var name = Path.GetFileNameWithoutExtension(file.Name.Substring(file.Name.IndexOf('-') + 1).Trim());
                var extenstion = file.Extension;
                var path = file.FullName;
                var book = new Book() {Author = author, Name = name, Extension = extenstion, Path = path};
                
                if (!_repo.Exists(x => x.Author == book.Author && x.Name == book.Name))
                {
                    _repo.Add(book);
                }
            }
        }
    }
}