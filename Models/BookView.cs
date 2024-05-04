using BookStorewithCRUD.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Models
{
    public class BookView
    {
        public IQueryable<Book> books {get; set;}
    }
}
