﻿using AutoMapper;
using BusinessLayer.Services;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    [Authorize]
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
        public bool CheckExistName(string Name) => categoryService.CheckExistName(Name);
        [HttpGet]
        public async Task<IActionResult> Get()=> 
            View("Categories", new CategoryPageModel() { Categories = await categoryService.GetAll() }) ;
        

        [HttpGet]
        public async Task<IActionResult> Delete(long id) {
            categoryService.Delete(id);
            return RedirectToAction("Get"); 
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(CategoryPageModel model)
        {
            if (ModelState.IsValid)
            {
                if (CheckExistName(model.EditModel.Name))
                {
                    TempData["Error"]="Категория с таким названием уже существует";
                    return RedirectToAction("Get");
                }
                var entity = _Mapper.Map<Category>(model.EditModel);
                entity.UserId = UserContext.UserId;
                await categoryService.Add(entity);
            }
            return RedirectToAction("Get");
        }


    }
}
