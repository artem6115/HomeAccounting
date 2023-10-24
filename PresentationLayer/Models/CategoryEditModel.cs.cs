using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Models
{
    public class CategoryEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "Название категории обязательное поле")]
        [MaxLength(15, ErrorMessage = "Максимальная длина - 15")]
        [RegularExpression(@".*[a-zA-Zа-яА-Я0-9_].*", ErrorMessage = "Строка пустая, либо содержит не корректные символы")]
        [Remote(action: "CheckExistName", controller: "Category", ErrorMessage = "Такое название категории уже существует")]


        public string Name { get; set; }
    }
}
