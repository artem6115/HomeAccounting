using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class TransactionEditModel
    {
        public long? Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public long AccountId { get; set; }
        [Required]
        public bool IsIncom { get; set; }
        public long? CategoryId { get; set; }
        [Required]
        public double? Value { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string? Comment { get; set; }

    }
}
