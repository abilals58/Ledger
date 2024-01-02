using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services
{
    public interface IDailyStockService
    {
        Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync();
        Task<DailyStock> GetDailyStockByDateAsync(int id, DateTime date);
        Task<DailyStock> AddDailyStockAsync(DailyStock dailystock);
        Task<DailyStock> UpdateDailyStockAsync(int id,DateTime date, DailyStock newdailystock);
        Task<DailyStock> DeleteDailyStockAsync(int id,DateTime date);

        Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id);
    }
    
    public class DailyStockService : IDailyStockService
    {
        private readonly IDailyStockRepository _dailyStockRepository;
        private IUnitOfWork _unitOfWork;

        public DailyStockService(IDailyStockRepository dailyStockRepository, IUnitOfWork unitOfWork)
        {
            _dailyStockRepository = dailyStockRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync()
        {
            return await _dailyStockRepository.GetAllDailyStocksAsync();
        }

        public async Task<DailyStock> GetDailyStockByDateAsync(int id,DateTime date)
        {
            return await _dailyStockRepository.GetDailyStockByDateAsync(id,date);
        }

        public async Task<DailyStock> AddDailyStockAsync(DailyStock dailystock)
        {
            try
            {
                await _dailyStockRepository.AddDailyStockAsync(dailystock);
                await _unitOfWork.CommitAsync();
                return dailystock;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        public async Task<DailyStock> UpdateDailyStockAsync(int id, DateTime date, DailyStock newdailystock)
        {
            try
            {
                var dailystock = await _dailyStockRepository.UpdateDailyStockAsync(id, date, newdailystock);
                await _unitOfWork.CommitAsync();
                return dailystock;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<DailyStock> DeleteDailyStockAsync(int id, DateTime date)
        {
            try
            {
                var dailystock = await _dailyStockRepository.DeleteDailyStockAsync(id, date);
                await _unitOfWork.CommitAsync();
                return dailystock;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id)
        {
            return await _dailyStockRepository.GetDailyStocksOfAStock(id);
        }
    }
}