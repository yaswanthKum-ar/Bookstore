using BookStorewithCRUD.Models.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Repositories.Abstract
{
    public interface IAuthorService
    {
        bool Add(Author model);
        bool Update(Author model);
        bool Delete(int id);
        Author FindbyId(int id);
        IEnumerable<Author> GetAll();
        Task<int> GetAuthorsExcel(IFormFile formFile);
    }
}
