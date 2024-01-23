namespace Ledger.Ledger.Web.Models;

public class BuyOrderMatch
{
    
    public int BuyOrderId { get; set; }
    public int SellOrderId { get; set; }

    public BuyOrderMatch()
    {
        
    }

    public BuyOrderMatch(int buyOrderId, int sellOrderId)
    {
        BuyOrderId = buyOrderId;
        SellOrderId = sellOrderId;
    }
}