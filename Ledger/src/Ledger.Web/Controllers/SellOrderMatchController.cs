using System;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Ledger.Web.Controllers
{
    [Route("api.ledger.com/v1.0.0/sellordermatchs")]
    [ApiController]
    public class SellOrderMatchController : ControllerBase
    {
        private readonly ISellOrderMatchService _sellOrderMatchService;

        public SellOrderMatchController(ISellOrderMatchService sellOrderMatchService)
        {
            _sellOrderMatchService = sellOrderMatchService;
        }
        
        // GET: api/dailystocks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _sellOrderMatchService.GetAllSellOrdersAsync());
        }
        
    }
}