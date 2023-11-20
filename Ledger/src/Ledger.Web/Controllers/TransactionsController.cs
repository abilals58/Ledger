using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase // This corresponds to the presentation tier and responsible for getting and sending http requests.
    {
        private readonly ITransactionRepository _transactionService;

        public TransactionsController(ITransactionRepository transactionService)
        {
            _transactionService = transactionService;
        }
        // GET: api/transactions
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _transactionService.GetAlTransactionsAsync());
        }

        // GET: api/transactions/id
        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<IActionResult> Get(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound("Transaction with given id does not exist in the database!");
            }

            return Ok(transaction);
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transaction transaction)
        {
            await _transactionService.AddTransactionAsync(transaction);
            return StatusCode(201, new {Message = "New Transaction added to the database!", Transaction = transaction});
        }

        // PUT: api/transactions/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Transaction newtransaction)
        {
            var transaction = await _transactionService.UpdateTransactionAsync(id, newtransaction);
            if (transaction == null)
            {
                return NotFound("Transaction with given id does not exist in the database!");
            }
            return Ok(new {Message = "Transaction with given id updated successfuly!", Transaction = transaction});
        }

        // DELETE: api/transactions/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _transactionService.DeleteTransactionAsync(id);
            if (transaction == null)
            {
                return NotFound("Transaction with given id does not exist in the database!");
            }
            
            return Ok(new {Message = "Transaction with given id deleted successfuly!", Transaction = transaction});
        }
    }
}
