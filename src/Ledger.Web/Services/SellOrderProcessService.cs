using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services;

public interface ISellOrderProcessService
{
    Task<SellOrderProcess> GetFifoSellOrderProcess();
    Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId);

}

public class SellOrderProcessService : ISellOrderProcessService
{
    private ISellOrderProcessRepository _sellOrderProcessRepository;
    private IUnitOfWork _unitOfWork;

    public SellOrderProcessService(ISellOrderProcessRepository sellOrderProcessRepository, IUnitOfWork unitOfWork)
    {
        _sellOrderProcessRepository = sellOrderProcessRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<SellOrderProcess> GetFifoSellOrderProcess()
    {
        return await _sellOrderProcessRepository.GetFifoSellOrder();
    }

    public async Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId)
    {
        var sellOrderProcess = await _sellOrderProcessRepository.UpdateOrderNum(sellOrderProcessId);
        await _unitOfWork.SaveChangesAsync();
        return sellOrderProcess;
    }
}