﻿using Microsoft.AspNetCore.Mvc;
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
        public StatisticController(IStatisticRepository statisticService)
        {
            _statisticRepository = statisticService;
        }

        // GET: api/<StatisticController>




        [HttpGet]
        [Route("category")]


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

            return await _statisticRepository.BuildCategoriesStatistic(filter);
        }

        [HttpGet]
        [Route("transaction")]
        public async Task<IEnumerable<StatisticData>> Transaction([FromQuery] StatisticFilter filter)
        {
            return await _statisticRepository.BuildTransactionStatistic(filter);
        }

        [HttpGet]
        [Route("balance")]
        public async Task<IEnumerable<StatisticData>> Balance([FromQuery] StatisticFilter filter)
        {
            return await _statisticRepository.BuildBalanceStatistic(filter);
        }

    }
}
