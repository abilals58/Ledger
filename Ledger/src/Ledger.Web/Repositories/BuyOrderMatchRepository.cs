using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories;

    public interface IBuyOrderMatchRepository
    {
        Task<BuyOrderMatch> AddBuyOrderMatchAsync(int buyOrderId, int sellOrderId);
        Task<IEnumerable<int>> GetMatchedSellOrders(int buyOrderId);
    }

    public class BuyOrderMatchRepository : IBuyOrderMatchRepository
    {
        private readonly DbSet<BuyOrderMatch> _dbBuyOrderMatch;

        public BuyOrderMatchRepository(IDbContext dbContext)
        {
            _dbBuyOrderMatch = dbContext.BuyOrderMatches;
        }
        
        public async Task<BuyOrderMatch> AddBuyOrderMatchAsync(int buyOrderId, int sellOrderId)
        {
            var buyOrderMatch = new BuyOrderMatch(buyOrderId, sellOrderId);
            await _dbBuyOrderMatch.AddAsync(buyOrderMatch);
            return buyOrderMatch;
        }

        public async Task<IEnumerable<int>> GetMatchedSellOrders(int buyOrderId)
        {
            return await _dbBuyOrderMatch.Where(b => b.BuyOrderId == buyOrderId).Select(b => b.SellOrderId).ToListAsync();
        }
    }