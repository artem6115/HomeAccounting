using DataLayer.Entities;
using DataLayer.Models;

namespace PresentationLayer.Models
{
    public class SettingsViewModel
    {
        public List<Account> Accounts { get; set; }
        public string Url { get; set; }
        public Settings settings { get; set; }

    }
}
