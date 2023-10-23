using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class AccountEditModel
    {
        public long? Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
