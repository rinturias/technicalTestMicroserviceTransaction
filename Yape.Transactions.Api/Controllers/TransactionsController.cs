using Microsoft.AspNetCore.Mvc;
using Yape.Transactions.Application.DTO;
using Yape.Transactions.Application.Interfaces;

namespace Yape.Transactions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDto dto)
        {

            var response = await _transactionService.CreateTransactionAsync(dto);
            return Ok(response);

        }
    }
}
