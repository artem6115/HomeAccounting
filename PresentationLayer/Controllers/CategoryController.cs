using AutoMapper;
using BusinessLayer.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService categoryService;
        private readonly ILogger<CategoryController> Log;
        private readonly IMapper _Mapper;

        public CategoryController(ILogger<CategoryController> log, CategoryService rep, IMapper maper)
        {
            categoryService = rep;
            _Mapper = maper;
            Log = log;
        }
        [HttpGet]
        public async Task<IActionResult> Get()=> View("Categories",await categoryService.GetAll());

        [HttpGet]
        public async Task<IActionResult> Delete(long id) {
            categoryService.Delete(id);
            return RedirectToAction("Get", await categoryService.GetAll()); 
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                await categoryService.Add(new Category() { Name = model.Name });
            }
            return RedirectToAction("Get", await categoryService.GetAll());
        }


    }
}
