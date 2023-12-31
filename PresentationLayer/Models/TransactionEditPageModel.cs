using DataLayer.Entities;

namespace PresentationLayer.Models
{
    public class TransactionEditPageModel
    {
        public TransactionEditModel EditModel { get; set; }
        public List<Account>? Accounts { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
