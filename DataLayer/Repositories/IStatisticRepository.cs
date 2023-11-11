using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public interface IStatisticRepository
    {
        public Task<IEnumerable<StatisticData>> BuildCategoriesStatistic(StatisticFilter filter);
        public Task<IEnumerable<StatisticData>> BuildTransactionStatistic(StatisticFilter filter);
        public Task<IEnumerable<StatisticData>> BuildBalanceStatistic(StatisticFilter filter);

    }
}
