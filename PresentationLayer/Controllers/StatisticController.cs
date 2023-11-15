using Microsoft.AspNetCore.Mvc;
using DataLayer.Models;
using BusinessLayer.Services;
using DataLayer.Repositories;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly ILogger<StatisticController> _logger;
        public StatisticController(IStatisticRepository statisticService, ILogger<StatisticController> log)
        {
            _statisticRepository = statisticService;
            _logger = log;
           
        }

        [HttpGet]
        [Route("category")]

        //Example query -
        //https://localhost:7177/api/statistic/category   - empty
        //https://localhost:7177/api/statistic/category?allaccounts=true
        //https://localhost:7177/api/statistic/category?allaccounts=true&alltime=true&year=2023&month=11
        public async Task<IEnumerable<StatisticData>> Category([FromQuery] StatisticFilter filter)
        {
            //filter = new StatisticFilter()
            //{
            //    AllAccounts = true,
            //    AccountId=72,
            //    AllTime = true,
            //    TypeTransaction = DataLayer.Enum.TypeTransaction.Expense,
            //    Month=11,
            //    Year=2023,
            //    TypeGroup=DataLayer.Enum.TypeGroup.Day

            //};
            _logger.LogDebug("Get запрос к api контроллеру, метод расчета категорий");
            return await _statisticRepository.BuildCategoriesStatistic(filter);
        }

        [HttpGet]
        [Route("transaction")]
        public async Task<IEnumerable<StatisticData>> Transaction([FromQuery] StatisticFilter filter)
        {
            _logger.LogDebug("Get запрос к api контроллеру, метод расчета транзакций");
            return await _statisticRepository.BuildTransactionStatistic(filter);
        }

        [HttpGet]
        [Route("balance")]
        public async Task<IEnumerable<StatisticData>> Balance([FromQuery] StatisticFilter filter)
        {
            _logger.LogDebug("Get запрос к api контроллеру, метод расчета баланса");
            return await _statisticRepository.BuildBalanceStatistic(filter);
        }

    }
}
