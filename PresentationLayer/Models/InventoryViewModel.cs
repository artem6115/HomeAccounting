using DataLayer.Entities;

namespace PresentationLayer.Models
{
    public class InventoryViewModel
    {
        public List<Inventory> Inventories { get; set; }
        public InventoryEditModel InventoryEditModel { get; set; }

        // Расчетный баланс
        public double CalculateBalance { get; set; }
        public Account Account { get; set; }

        //Адресс сервера, схема + адрес
        public string Url { get; set; }
    }
}
