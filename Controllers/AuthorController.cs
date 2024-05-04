using BookStorewithCRUD.Models.Domain;
using BookStorewithCRUD.Repositories.Abstract;
using BookStorewithCRUD.Repositories.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStorewithCRUD.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _service;
        public AuthorController(IAuthorService AuthorService)
        {
            this._service = AuthorService;
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Author model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _service.Add(model);
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
            var record = _service.FindbyId(id);
            return View(record);
        }
        [HttpPost]
        public IActionResult Update(Author model)
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
        [Authorize(Roles = "Admin")]
        public IActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UploadExcel(IFormFile excelfile)
        {
            if(excelfile ==null || excelfile.Length==0)
            {
                ViewBag.Errormessage = "Please select an xlsx file";
                return View();
            }
            if(!Path.GetExtension(excelfile.FileName).Equals(".xlsx",StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Errormessage = "Unsupported File format, xlsx is expected";
                return View();
            }
            int authorcount = await _service.GetAuthorsExcel(excelfile);
            ViewBag.message = $"{authorcount} Author names added";
            return View();
        }
    }
}
