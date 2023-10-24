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
        [RegularExpression(@".*\w.*",ErrorMessage ="Строка пустая, либо содержит не корректные символы")]
        public string Name { get; set; }
    }
}
