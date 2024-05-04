using BookStorewithCRUD.Models.Domain;
using BookStorewithCRUD.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _service;
        public GenreController(IGenreService genreService)
        {
            this._service = genreService;
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Genre model)
        {
            if(!ModelState.IsValid)
            return View(model);

            var result = _service.Add(model);
            if(result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            TempData["msg"] = "Error has occured on server side";
            return View(model);
        }


        public IActionResult Update(int id)
        {
            var record = _service.FindbyId(id);
            return View(record);
        }
        [HttpPost]
        public IActionResult Update(Genre model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _service.Update(model);
            if (result)
            {
                return RedirectToAction("GetAll");
            }
            TempData["msg"] = "Error has occured on server side";
            return View(model);
        }


        public IActionResult Delete(int id)
        {
            
            var result = _service.Delete(id);
            return RedirectToAction("GetAll");
            
        }
        public IActionResult GetAll()
        {
            var data = _service.GetAll();
            return View(data);
        }
    }
}
