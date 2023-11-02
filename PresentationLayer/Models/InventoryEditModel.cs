using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class InventoryEditModel
    {
        [Required]
        public long AccountId { get; set; }
        [Required(ErrorMessage ="Это обязательное поле")]
        public DateTime Date { get; set; }
        [RegularExpression(@"^(-)?\d+(,\d{1,2})?", ErrorMessage = "Число содержать не более 2 знаков после запятой")]
        public string? Value { get; set; }

    }
}
