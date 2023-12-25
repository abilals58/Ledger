using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api.ledger.com/v1.0.0/sellorders")]
    [ApiController]
    public class SellOrdersController : ControllerBase // This corresponds to the presentation tier and responsible for getting and sending http requests.
    {
        private readonly ISellOrderService _sellOrderService;

        public SellOrdersController(ISellOrderService sellOrderService)
        {
            _sellOrderService = sellOrderService;
        }
        //CRUD OPERATIONS
        
        // GET: api/sellorders
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _sellOrderService.GetAllSellOrdersAsync());
        }

        // GET: api/sellorders/id
        [HttpGet("{id}", Name = "GetSellOrder")]
        public async Task<IActionResult> Get(int id)
        {
            var sellOrder = await _sellOrderService.GetSellOrderByIdAsync(id);
            if (sellOrder == null)
            {
                return NotFound("SellOrder with given id does not exist in the database!");
            }

            return Ok(sellOrder);
        }

        // POST: api/sellorders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SellOrder sellOrder)
        {
            await _sellOrderService.AddSellOrderAsync(sellOrder);
            return StatusCode(201, new{Message = "New SellOrder added to the database succesfuly!", SellOrder = sellOrder});
        }

        // PUT: api/sellorders/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SellOrder newSellOrder)
        {
            var sellOrder = await _sellOrderService.UpdateSellOrderAsync(id, newSellOrder);
            if (sellOrder == null)
            {
                return NotFound("SellOrder with given id does not exist in the database!");
            }
            return Ok(new {Message = "SellOrder with given id updated successfuly", SellOrder = sellOrder});
        }

        // DELETE: api/sellorders/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sellOrder = await _sellOrderService.DeleteSellOrderAsync(id);
            if (sellOrder == null)
            {
                return NotFound("SellOrder with given id does not exist in the database!");
            }
            
            return Ok(new {Message = "SellOrder with given id deleted successfuly!", SellOrder = sellOrder});
        }
        
        //BUSSINESS OPERATIONS
        [HttpGet("{buyorderId}/{bidSize}")]

        public async Task<IActionResult> MatchedSellOrders(int buyorderId, int bidSize)
        {
            var sellOrders = await _sellOrderService.MatchSellOrdersAsync(buyorderId, bidSize);
            if (sellOrders.Count == 0)
            {
                return NotFound("No match with given stock and price!");
            }

            return Ok(new { Message = "Some matches are found!", MatchList = sellOrders });
        }
    }
}
