using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class CategoryEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage ="Название категории обязательное поле")]
        [MaxLength(15, ErrorMessage = "Максимальная длина - 15")]
        [RegularExpression(@"/^\w+$/", ErrorMessage = "Строка пустая, либо содержит не корректные символы")]


        public string Name { get; set; }
    }
}
