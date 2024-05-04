using BookStorewithCRUD.Models.Domain;
using BookStorewithCRUD.Repositories.Abstract;
using BookStorewithCRUD.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace BookStorewithCRUD.Repositories.Implementation
{
    public class BookService:IBookService
    {
        private readonly DatabaseContext context;
        public BookService(DatabaseContext context)
        {
            this.context = context;
        }
        public bool Add(Book model)
        {
            try
            {
                context.Book.Add(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.FindbyId(id);
                if (data == null)
                    return false;
                context.Book.Remove(data);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Book FindbyId(int id)
        {
            return context.Book.Find(id);
        }



        public IQueryable<Book> GetAll()
        {
            var data = (from book in context.Book
                               join author in context.Author on book.Authorid equals author.Id
                               join publisher in context.Publisher on book.Publisherid equals publisher.Id
                               join genre in context.Genre on book.Genreid equals genre.Id
                               select new Book
                               {
                                   Id = book.Id,
                                   Authorid = book.Authorid,
                                   Genreid = book.Genreid,
                                   Isbn = book.Isbn,
                                   Publisherid = book.Publisherid,
                                   Title = book.Title,
                                   Totalpages = book.Totalpages,
                                   Genrename = genre.Genrename,
                                   Authorname = author.Authorname,
                                   Publishername = publisher.Publishername
                               }
                       );

            return data;
        }

        public MemoryStream GetBooksExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Books");
                excelWorksheet.Cells["A1"].Value = "Title";
                excelWorksheet.Cells["B1"].Value = "Genre Name";
                excelWorksheet.Cells["C1"].Value = "ISBN";
                excelWorksheet.Cells["D1"].Value = "Total Pages";
                excelWorksheet.Cells["E1"].Value = "Author Name";
                excelWorksheet.Cells["F1"].Value = "Publisher Name";

                int row = 2;
                var data = (from book in context.Book
                            join author in context.Author on book.Authorid equals author.Id
                            join publisher in context.Publisher on book.Publisherid equals publisher.Id
                            join genre in context.Genre on book.Genreid equals genre.Id
                            select new Book
                            {
                                Id = book.Id,
                                Authorid = book.Authorid,
                                Genreid = book.Genreid,
                                Isbn = book.Isbn,
                                Publisherid = book.Publisherid,
                                Title = book.Title,
                                Totalpages = book.Totalpages,
                                Genrename = genre.Genrename,
                                Authorname = author.Authorname,
                                Publishername = publisher.Publishername
                            }
                        );
                foreach(Book book1 in data)
                {
                    excelWorksheet.Cells[row, 1].Value = book1.Title;
                    excelWorksheet.Cells[row, 2].Value = book1.Genrename;
                    excelWorksheet.Cells[row, 3].Value = book1.Isbn;
                    excelWorksheet.Cells[row, 4].Value = book1.Totalpages;
                    excelWorksheet.Cells[row, 5].Value = book1.Authorname;
                    excelWorksheet.Cells[row, 6].Value = book1.Publishername;
                    row++;

                }
                excelWorksheet.Cells[$"A1:H{row}"].AutoFitColumns();
                excelPackage.Save();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        public IQueryable<Book> GetSearched(string searchby, string searchvalue)
        {
            var data = (from book in context.Book
                        join author in context.Author on book.Authorid equals author.Id
                        join publisher in context.Publisher on book.Publisherid equals publisher.Id
                        join genre in context.Genre on book.Genreid equals genre.Id
                        select new Book
                        {
                            Id = book.Id,
                            Authorid = book.Authorid,
                            Genreid = book.Genreid,
                            Isbn = book.Isbn,
                            Publisherid = book.Publisherid,
                            Title = book.Title,
                            Totalpages = book.Totalpages,
                            Genrename = genre.Genrename,
                            Authorname = author.Authorname,
                            Publishername = publisher.Publishername
                        }
                        );
            

            switch (searchby)
            {
                case "Book":
                    var Bookdata = data.Where(temp => (!string.IsNullOrEmpty(searchvalue) ? temp.Title.Contains(searchvalue, StringComparison.OrdinalIgnoreCase) : true));
                    return Bookdata;
                case nameof(Book.Authorname):
                    var Authordata = data.Where(temp => (!string.IsNullOrEmpty(searchvalue) ? temp.Authorname.Contains(searchvalue, StringComparison.OrdinalIgnoreCase) : true));
                    return Authordata;
                case nameof(Book.Genrename):
                    var Genredata = data.Where(temp => (!string.IsNullOrEmpty(searchvalue) ? temp.Genrename.Contains(searchvalue, StringComparison.OrdinalIgnoreCase) : true));
                    return Genredata;
                case nameof(Book.Publishername):
                    var Publisherdata = data.Where(temp => (!string.IsNullOrEmpty(searchvalue) ? temp.Publishername.Contains(searchvalue, StringComparison.OrdinalIgnoreCase) : true));
                    return Publisherdata;
            }
            return data;
        }

        public IQueryable<Book> GetSorted(IQueryable<Book> books, string sortby, SortOrderOptions sorttype)
        {
            var sorteddata = books;

            if (string.IsNullOrEmpty(sortby))
            {
                return books;
            }

            switch(sortby,sorttype)
            {
                case (nameof(Book.Title), SortOrderOptions.ASC):
                       sorteddata = books.OrderBy(temp => temp.Title);
                    break;
                case (nameof(Book.Title), SortOrderOptions.DESC):
                    sorteddata = books.OrderByDescending(temp => temp.Title);
                    break;

                case (nameof(Book.Authorname), SortOrderOptions.ASC):
                    sorteddata = books.OrderBy(temp => temp.Authorname);
                    break;
                case (nameof(Book.Authorname), SortOrderOptions.DESC):
                    sorteddata = books.OrderByDescending(temp => temp.Authorname);
                    break;
            }

            //var Sortedata = (sortby, sorttype) switch
            //{
            //    (nameof(Book.Title), SortOrderOptions.ASC) => books.OrderBy(temp => temp.Title, StringComparer.OrdinalIgnoreCase),
            //    (nameof(Book.Title), SortOrderOptions.DESC) => books.OrderByDescending(temp => temp.Title, StringComparer.OrdinalIgnoreCase),
            //    (nameof(Book.Authorname), SortOrderOptions.ASC) => books.OrderBy(temp => temp.Authorname, StringComparer.OrdinalIgnoreCase),
            //    (nameof(Book.Authorname), SortOrderOptions.DESC) => books.OrderByDescending(temp => temp.Authorname, StringComparer.OrdinalIgnoreCase),
            //    (nameof(Book.Genrename), SortOrderOptions.ASC) => books.OrderBy(temp => temp.Genrename, StringComparer.OrdinalIgnoreCase),
            //    (nameof(Book.Genrename), SortOrderOptions.DESC) => books.OrderByDescending(temp => temp.Genrename, StringComparer.OrdinalIgnoreCase),
            //    (nameof(Book.Publishername), SortOrderOptions.ASC) => books.OrderBy(temp => temp.Publishername, StringComparer.OrdinalIgnoreCase),
            //    (nameof(Book.Publishername), SortOrderOptions.DESC) => books.OrderByDescending(temp => temp.Publishername, StringComparer.OrdinalIgnoreCase),



            //};
            return sorteddata;
        }

        public bool Update(Book model)
        {
            try
            {
                context.Book.Update(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
