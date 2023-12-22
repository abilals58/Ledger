using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<Stock> GetStockByIdAsync(int id);
        Task AddStockAsync(Stock stock);
        Task<Stock> UpdateStockAsync(int id, Stock newStock);
        Task<Stock> DeleteStockAsync(int id);
        Task<List<List<object>>> RetrieveAllStockInfo();

        Task<Stock> UpdateAStockPrice(int id, double newPrice); // updates current, highest and lowest price values of a stock
    }
    public class StockService :IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<IEnumerable<Stock>> GetAllStocksAsync()
        {
            return await _stockRepository.GetAllStocksAsync();
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            return await _stockRepository.GetStockByIdAsync(id);
        }

        public async Task AddStockAsync(Stock stock)
        {
            await _stockRepository.AddStockAsync(stock);        
        }

        public async Task<Stock> UpdateStockAsync(int id, Stock newStock)
        {
            return await _stockRepository.UpdateStockAsync(id, newStock);        
        }

        public async Task<Stock> DeleteStockAsync(int id)
        {
            return await _stockRepository.DeleteStockAsync(id);        
        }

        public async Task<List<List<object>>> RetrieveAllStockInfo()
        {
            var stocks = await _stockRepository.GetAllStocksAsync();
            var allStocksData = new List<List<object>>();
            
            foreach (var stock in stocks)
            {
                if (stock.Status) // if stock is active 
                {
                    var aStockData = new List<object>();
                    //retrieving desired info related to a single stock
                    aStockData.Add(stock.StockName); //name of the stock
                    aStockData.Add(stock.CurrentPrice); //currentPrice
                    aStockData.Add(stock.HighestPrice); //hightest price
                    aStockData.Add(stock.LowestPrice); //lowest price
                    aStockData.Add(stock.InitialStock); // total number of lots 
                    aStockData.Add(stock.InitialStock * stock.CurrentPrice); //volume (dynamic total value)
                    
                    allStocksData.Add(aStockData);//add a stocks data to allstocks data
                }
            }
            return allStocksData;
        }

        

        /*public async Task<Stock> UpdateAStockPrice(int id,double newPrice)
        {
            var stock = await _stockRepository.GetStockByIdAsync(id);
            if (stock == null) return null;
            
            if (newPrice < stock.LowestPrice) //update lowest price and current price
            {
                var newStock = new Stock(stock.StockId, stock.StockName, stock.InitialStock, stock.InitialPrice,
                    stock.CurrentStock, newPrice, stock.HighestPrice, newPrice, stock.Status);
                return await _stockRepository.UpdateStockAsync(stock.StockId, newStock);
            }

            if (newPrice > stock.HighestPrice) //update highest price and current price
            {
                var newStock = new Stock(stock.StockId, stock.StockName, stock.InitialStock, stock.InitialPrice,
                    stock.CurrentStock, newPrice, newPrice, stock.LowestPrice, stock.Status);
                return await _stockRepository.UpdateStockAsync(stock.StockId, newStock);

            }
            else // update only current price
            {
                var newStock = new Stock(stock.StockId, stock.StockName, stock.InitialStock, stock.InitialPrice,
                    stock.CurrentStock, newPrice, stock.HighestPrice, stock.LowestPrice, stock.Status);
                return await _stockRepository.UpdateStockAsync(stock.StockId, newStock);
            }
        }*/
        /*public async Task<Stock> UpdateAStockPrice(int id,double newPrice)
        {
            var stock = await _stockRepository.GetStockByIdAsync(id);
            if (stock == null) return null;
            
            if (newPrice < stock.LowestPrice) //update lowest price and current price as newPrice
            {
                return await _stockRepository.UpdateStockPriceAsync(id, newPrice, stock.HighestPrice, newPrice);
            }

            if (newPrice > stock.HighestPrice) //update highest price and current price as newPrice
            {
                return await _stockRepository.UpdateStockPriceAsync(id, newPrice, newPrice, stock.LowestPrice);
            }
            else // update only current price
            {
                var newStock = new Stock(stock.StockId, stock.StockName, stock.InitialStock, stock.InitialPrice,
                    stock.CurrentStock, newPrice, stock.HighestPrice, stock.LowestPrice, stock.Status);
                return await _stockRepository.UpdateStockPriceAsync(id, newPrice, stock.HighestPrice, stock.LowestPrice);
            }
        }*/
        
        public async Task<Stock> UpdateAStockPrice(int id, double newPrice)
        {
            return await _stockRepository.UpdateStockPriceAsync(id, newPrice);
        }
    }
}