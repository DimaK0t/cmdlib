using System.Collections.Generic;
using NoSql.Models.DbModels;

namespace NoSql.DAL
{
    public interface ILibrarian
    {
        IEnumerable<Book> GetBooks();
        void UpdateDbLiblirary(bool cleanBefore);
    }
}