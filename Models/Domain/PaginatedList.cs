using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Models.Domain
{
    public class PaginatedList<T> :List<T>
    {
        public int Pageindex { get; set; }
        public int Totalpages { get; set; }

        public PaginatedList(List<T> items, int count, int pageindex, int pagesize)
        {
            Pageindex = pageindex;
            Totalpages = (int)Math.Ceiling(count / (double)pagesize);
            this.AddRange(items);
        }
        public bool PreviousPage
        {
            get
            {
                return (Pageindex > 1);
            }
        }
        public bool NextPage
        {
            get
            {
                return (Pageindex < Totalpages);
            }
        }
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageindex, int pagesize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
            return new PaginatedList<T>(items, count, pageindex, pagesize);
        }
    }
}
