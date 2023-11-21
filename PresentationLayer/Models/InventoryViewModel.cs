using DataLayer.Entities;

namespace PresentationLayer.Models
{
    public class InventoryViewModel
    {
        public List<Inventory> Inventories { get; set; }
        public InventoryEditModel InventoryEditModel { get; set; }
        public double CalculateValue { get; set; }
        public Account Account { get; set; }
        public string Url { get; set; }
    }
}
