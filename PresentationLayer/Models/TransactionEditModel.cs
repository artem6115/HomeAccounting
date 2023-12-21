using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PresentationLayer.Models
{
    public class TransactionEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "Наличие счета обязательно (кликните для добовления)")]
        public long AccountId { get; set; }
        [Required(ErrorMessage = "Обязательно укажите тип операции")]
        public bool IsIncome { get; set; }
        public long? CategoryId { get; set; }
        [Required(ErrorMessage = "Величина операции обязательно поле")]
        [RegularExpression(@"^\d+(,\d{1,2})?", ErrorMessage ="Число должно быть положительным и содержать не более 2 знаков после запятой")]

        public string Value { get; set; }
        [Required(ErrorMessage = "Дата обязательно поле")]
        [DataType(DataType.DateTime,ErrorMessage ="Дата задана не корректно")]
        public DateTime Date { get; set; }
        [MaxLength(40,ErrorMessage ="Максимальная длинна 40 символов")]
        public string? Comment { get; set; }

    }
}
