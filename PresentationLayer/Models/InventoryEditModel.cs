using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class InventoryEditModel
    {
        [Required]
        public long AccountId { get; set; }
        [Required(ErrorMessage ="Это обязательное поле")]
        [DataType(DataType.DateTime)]
        [Remote(action: "CheckExistData", controller: "Inventory", ErrorMessage = "На одну дату можно создать не более 1 инвенторизации")]

        public DateTime Date { get; set; }
        [RegularExpression(@"^(-)?\d+(,\d{1,2})?", ErrorMessage = "Число может содержать не более 2 знаков после запятой")]
        public string? Value { get; set; }

        //Создать ли корректирующую транзакцию
        public bool CreateBalanceTransaction { get; set; } = true;

    }
}
