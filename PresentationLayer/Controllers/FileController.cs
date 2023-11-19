using DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using DataLayer.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PresentationLayer.Controllers
{
    [Route("api/File")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly ILogger<FileController> _logger;
        public FileController(TransactionService transactionService, ILogger<FileController> log)
        {
            _transactionService = transactionService;
            _logger = log;

        }
        [Route("excel")]
        [HttpGet]
        //https://localhost:7177/api/File?AccountId=85&MoreValue=False&IsForward=False&TypeTransaction=IncomeAndExpense
        public async Task<IActionResult> Excel([FromQuery] Filter filter)
        {
            var stream =await _transactionService.GetExcel(filter);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "Transactions.xls");
            
        }
        [HttpGet]
        [Route("word")]

        public async Task<IActionResult> Word([FromQuery] Filter filter)
        {
            _transactionService.GetWord(filter);
            
            return RedirectToAction("Index","Home");

        }
    }
}
