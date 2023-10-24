using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class TransactionEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage ="Название счета обязательно поле")]
        public long AccountId { get; set; }
        [Required(ErrorMessage = "Обязательно укажите тип операции")]
        public bool IsIncom { get; set; }
        public long? CategoryId { get; set; }
        [Required(ErrorMessage = "Величина операции обязательно поле")]
        [RegularExpression(@"/ ^\d + (\.\d\d)?$/",ErrorMessage ="Число должно содержать не более 2 знаков после запятой")]
        [Range(1,double.MaxValue,ErrorMessage ="Число должно быть больше 1")]
        public double? Value { get; set; }
        [Required(ErrorMessage = "Дата обязательно поле")]
        [DataType(DataType.Date,ErrorMessage ="Дата задана не корректно")]
        public DateTime Date { get; set; }
        public string? Comment { get; set; }

    }
}
