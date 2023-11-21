using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    
    public interface IStocksOfUserService
    {
        Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync();
        Task<StocksOfUser> GetStocksOfUserByIdAsync(int id);
        Task AddStocksOfUserAsync(StocksOfUser stocksOfUser);
        Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser);
        Task<StocksOfUser> DeleteStocksOfUserAsync(int id);
    }
    public class StocksOfUserService : IStocksOfUserService
    {

        private readonly IStocksOfUserRepository _stocksOfUserRepository;

        public StocksOfUserService(IStocksOfUserRepository stocksOfUserRepository)
        {
            _stocksOfUserRepository = stocksOfUserRepository;
        }
        public async Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync()
        {
            return await _stocksOfUserRepository.GetAllStocksOfUserAsync();
        }

        public async Task<StocksOfUser> GetStocksOfUserByIdAsync(int id)
        {
            return await _stocksOfUserRepository.GetStocksOfUserByIdAsync(id);        
        }

        public async Task AddStocksOfUserAsync(StocksOfUser stocksOfUser)
        {
            await _stocksOfUserRepository.AddStocksOfUserAsync(stocksOfUser);        
        }

        public async Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser)
        {
            return await _stocksOfUserRepository.UpdateStocksOfUserAsync(id, newStocksOfUser);        
        }

        public async Task<StocksOfUser> DeleteStocksOfUserAsync(int id)
        {
            return await _stocksOfUserRepository.DeleteStocksOfUserAsync(id);        
        }
    }
}