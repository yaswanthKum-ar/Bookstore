using BookStorewithCRUD.Models.Domain;
using BookStorewithCRUD.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Repositories.Abstract
{
    public interface IBookService
    {
        bool Add(Book model);
        bool Update(Book model);
        bool Delete(int id);
        Book FindbyId(int id);
        IQueryable<Book> GetAll();
        IQueryable<Book> GetSearched(string searchby, string searchvalue);
        IQueryable<Book> GetSorted(IQueryable<Book> books, string sortby, SortOrderOptions sorttype);
        MemoryStream GetBooksExcel();

    }
}
