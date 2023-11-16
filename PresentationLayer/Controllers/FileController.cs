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

        [HttpGet]
        public FileStreamResult Excel([FromQuery] Filter filter)
        {
            var stream = _transactionService.GetExcel(filter).Result;
            return File(stream, "application/octet-stream", "Transactions.xlsm");
        }
    }
}
