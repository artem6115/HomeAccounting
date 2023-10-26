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

        public CategoryController(ILogger<CategoryController> log, CategoryService rep, IMapper mapper)
        {
            categoryService = rep;
            _Mapper = mapper;
            Log = log;
        }
        public bool CheckExistName(string name) => !categoryService.CheckExistName(name);
        [HttpGet]
        public async Task<IActionResult> Get()=> View("Categories");

        [HttpGet]
        public async Task<IActionResult> Delete(long id) {
            categoryService.Delete(id);
            return RedirectToAction("Get"); 
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                await categoryService.Add(_Mapper.Map<Category>(model));
            }
            return RedirectToAction("Get");
        }


    }
}
