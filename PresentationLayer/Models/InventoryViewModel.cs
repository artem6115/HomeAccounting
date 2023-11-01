using DataLayer.Entities;

namespace PresentationLayer.Models
{
    public class InventoryViewModel
    {
        public List<Inventory> Inventories { get; set; }
        public long AccountId { get; set; }
    }
}
