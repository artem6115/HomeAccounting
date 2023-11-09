using DataLayer.Models;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class StatisticService
    {
        private readonly CategoryRepository _categoryRepository;
        public StatisticService(CategoryRepository ctg)
        {
            _categoryRepository = ctg;
        }
        public async Task<IEnumerable<StatisticData>> BuildCategoriesStatistic(StatisticFilter filter)
        {
           var categories = await _categoryRepository.GetAll();
            
        }
    }
}
