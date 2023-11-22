using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Models
{
    public class AccountEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "Название счета обязательное поле")]
        [MaxLength(30, ErrorMessage = "Максимальная длина - 30")]
        [RegularExpression(@".*[a-zA-Zа-яА-Я0-9_].*",ErrorMessage ="Строка пустая, либо содержит не корректные символы")]
        [Remote(action:"CheckExistName",controller:"Account",ErrorMessage ="Данное название счета уже существует")]
        public string Name { get; set; }

        [RegularExpression(@"^(-)?\d+(,\d{1,2})?", ErrorMessage = "Число должно содержать не более 2 знаков после запятой")]
        public string StartValue { get; set; }
    }
}
