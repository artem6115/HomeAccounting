using Microsoft.AspNetCore.Mvc;
using DataLayer.Models;
using BusinessLayer.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly StatisticService _statisticService;
        public StatisticController(StatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        // GET: api/<StatisticController>
        [HttpGet]
        public async Task<IEnumerable<StatisticData>> Category([FromBody] StatisticFilter filter)
        {
            return await _statisticService.BuildCategoriesStatistic(filter);
        }

    }
}
