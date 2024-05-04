using BookStorewithCRUD.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Repositories.Abstract
{
    public interface IGenreService
    {
        bool Add(Genre model);
        bool Update(Genre model);
        bool Delete(int id);
        Genre FindbyId(int id);
        IEnumerable<Genre> GetAll();
    }
}
