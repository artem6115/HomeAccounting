using DataLayer.Entities;
using DataLayer.Models;

namespace PresentationLayer.Models
{
    public class CategoryPageModel
    {
        public List<Category>? Categories { get; set; }
        public CategoryEditModel EditModel { get; set; } = new CategoryEditModel();

    }
}
