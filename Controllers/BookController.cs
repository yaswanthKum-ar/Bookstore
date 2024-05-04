using BookStorewithCRUD.Models.Domain;
using BookStorewithCRUD.Repositories.Abstract;
using BookStorewithCRUD.Repositories.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using BookStorewithCRUD.Models;
using Rotativa.AspNetCore;
using System.IO;

namespace BookStorewithCRUD.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookservice;
        private readonly IAuthorService authorService;
        private readonly IGenreService genreService;
        private readonly IPublisherService publisherService;
        private readonly DatabaseContext ctx;

        public BookController(IBookService BookService, IAuthorService AuthorService, IGenreService GenreService, IPublisherService PublisherService, DatabaseContext databaseContext)
        {
            this._bookservice = BookService;
            this.authorService = AuthorService;
            this.genreService = GenreService;
            this.publisherService = PublisherService;
            this.ctx = databaseContext;
        }
        public IActionResult Add()
        {
            var model = new Book();
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.Authorname, Value = a.Id.ToString() }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.Publishername, Value = a.Id.ToString() }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Genrename, Value = a.Id.ToString() }).ToList();

            return View(model);
        }
        [HttpPost]
        public IActionResult Add(Book model)
        {
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.Authorname, Value = a.Id.ToString(), Selected=a.Id==model.Authorid }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.Publishername, Value = a.Id.ToString(), Selected=a.Id == model.Publisherid }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Genrename, Value = a.Id.ToString(), Selected=a.Id == model.Genreid }).ToList();

            if (!ModelState.IsValid)
                return View(model);

            var result = _bookservice.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            TempData["msg"] = "Error has occured on server side";
            return View(model);
        }

       


        public IActionResult Update(int id)
        {
            var model = _bookservice.FindbyId(id);
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.Authorname, Value = a.Id.ToString(), Selected = a.Id == model.Authorid }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.Publishername, Value = a.Id.ToString(), Selected = a.Id == model.Publisherid }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Genrename, Value = a.Id.ToString(), Selected = a.Id == model.Genreid }).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(Book model)
        {
            model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.Authorname, Value = a.Id.ToString(), Selected = a.Id == model.Authorid }).ToList();
            model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.Publishername, Value = a.Id.ToString(), Selected = a.Id == model.Publisherid }).ToList();
            model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Genrename, Value = a.Id.ToString(), Selected = a.Id == model.Genreid }).ToList();
            if (!ModelState.IsValid)
                return View(model);

            var result = _bookservice.Update(model);
            if (result)
            {
                return RedirectToAction("GetAll");
            }
            TempData["msg"] = "Error has occured on server side";
            return View(model);
        }


        public IActionResult Delete(int id)
        {

            var result = _bookservice.Delete(id);
            return RedirectToAction("GetAll");

        }
        public async Task<IActionResult> GetAll(int ? i, string searchby, string searchvalue, string sortby = nameof(Book.Title), SortOrderOptions sortOrder = SortOrderOptions.ASC,int pagenumber = 1)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(Book.Authorname),"Author Name" },
                {"Book", "Book Name" },
                {nameof(Book.Genrename),"Genre Name" },
                {nameof(Book.Publishername), "Publisher Name" }

            };
            ViewBag.searchbyval = searchby;
            ViewBag.searchvalue = searchvalue;
            var serachdata = _bookservice.GetSearched(searchby, searchvalue);
            var sorteddata = _bookservice.GetSorted(serachdata, sortby, sortOrder);
            ViewBag.currentsortby = sortby;
            ViewBag.currentsortorder = sortOrder;
            //var data = _bookservice.GetAll();
            //return View(sorteddata);


            //return View(sorteddata.ToPagedList(i ?? 1,3));
            int pagesize = 4;
            var totalrecords = sorteddata.Count();
            int totalpages = (int)Math.Ceiling(totalrecords / (double)pagesize);
            var querydata = sorteddata.AsQueryable();
            ViewBag.pagesize = pagesize;
            ViewBag.totalpages = totalpages;
            ViewBag.totalrecords = totalrecords;
            return View(await PaginatedList<Book>.CreateAsync(querydata, pagenumber, pagesize));
        }

        public IActionResult BookPDF()
        {
            var data = _bookservice.GetAll();
            //return View(data);
            return new ViewAsPdf("BookPDF", data, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 }

            };
        }

        public IActionResult BookExcel()
        {
            MemoryStream memoryStream = _bookservice.GetBooksExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Books.xlsx");
        }
    }
}
