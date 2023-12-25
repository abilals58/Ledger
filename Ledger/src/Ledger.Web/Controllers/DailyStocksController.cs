using System;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api.ledger.com/v1.0.0/dailystocks")]
    [ApiController]
    public class DailyStocksController : ControllerBase // This corresponds to the presentation tier and responsible for getting and sending http requests.
    {
        private readonly IDailyStockService _dailyStockService;

        public DailyStocksController(IDailyStockService dailyStockService)
        {
            _dailyStockService = dailyStockService;
        }
        
        // GET: api/dailystocks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _dailyStockService.GetAllDailyStocksAsync());
        }

        // GET: api/dailystocks/id/date
        [HttpGet("{id}/{date}")]
        public async Task<IActionResult> Get(int id,DateTime date)
        {
            var dailyStock = await _dailyStockService.GetDailyStockByDateAsync(id,date);
            if (dailyStock == null)
            {
                return NotFound("Daily stock with given date does not exist in the database!");
            }
            return Ok(dailyStock);
        }

        // POST: api/dailystocks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DailyStock dailyStock)
        {
            var dailystock = await _dailyStockService.AddDailyStockAsync(dailyStock);
            return StatusCode(201, new {Message = "New DailyStock added to the database succesfuly!", DailyStock = dailystock});
        }
        
        // PUT: api/dailystocks/id/date
        [HttpPut("{id}/{date}")]
        public async Task<IActionResult> Put(int id, DateTime date, [FromBody] DailyStock newdailystock)
        {
            var dailyStock = await _dailyStockService.UpdateDailyStockAsync(id,date, newdailystock);
            if (dailyStock == null) 
            {
                return NotFound("DailyStock with given date does not exist in the database!");
            }
            return Ok(new {Message = "DailyStock with given date updated successfuly!", DailyStock = dailyStock});
        }

        // DELETE: api/dailystocks/id/date
        [HttpDelete("{id}/{date}")]
        public async Task<IActionResult> Delete(int id,DateTime date)
        {
            var dailyStock = await _dailyStockService.DeleteDailyStockAsync(id,date);
            if (dailyStock == null)
            {
                return NotFound("DailyStock with given date does not exist in the database!");
            }
            return Ok(new {Message = "DailyStock with given date deleted successfuly!", DailyStock = dailyStock});
        }
        
        // GET: api/dailystocks/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dailyStocks = await _dailyStockService.GetDailyStocksOfAStock(id);
            if (dailyStocks == null)
            {
                return NotFound("Daily stock with given id does not exist in the database!");
            }
            return Ok(dailyStocks);
        }
    }
}
