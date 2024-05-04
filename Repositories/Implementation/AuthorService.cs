using BookStorewithCRUD.Models.Domain;
using BookStorewithCRUD.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Repositories.Implementation
{
    public class AuthorService:IAuthorService
    {
        private readonly DatabaseContext context;
        public AuthorService(DatabaseContext context)
        {
            this.context = context;
        }
        public bool Add(Author model)
        {
            try
            {
                context.Author.Add(model);
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
                context.Author.Remove(data);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Author FindbyId(int id)
        {
            return context.Author.Find(id);
        }

        public IEnumerable<Author> GetAll()
        {
            return context.Author.ToList();
        }

        public async Task<int> GetAuthorsExcel(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int authorcount = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Authors"];
                int rowcount = excelWorksheet.Dimension.Rows;
                for(int row =2; row<=rowcount; row++)
                {
                    string cellvalue = Convert.ToString(excelWorksheet.Cells[row, 1].Value);
                    if(!string.IsNullOrEmpty(cellvalue))
                    {
                        string authorname = cellvalue;
                        if(context.Author.Where(temp => temp.Authorname== authorname).Count()==0)
                        {
                            Author author = new Author()
                            {
                                Authorname = authorname
                            };
                            context.Author.Add(author);
                            context.SaveChanges();
                            authorcount++;
                        }
                    }
                }
            }
            return authorcount;
        }

        public bool Update(Author model)
        {
            try
            {
                context.Author.Update(model);
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
