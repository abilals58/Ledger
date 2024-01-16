using System.ComponentModel.DataAnnotations;

namespace Ledger.Ledger.Web.Models
{
    public class SellOrderMatch
    {
        [Key]
        public int SellOrderId { get; set; }
        [Key]
        public int BuyOrderId { get; set; }

        public SellOrderMatch()
        {
            
        }

        public SellOrderMatch(int sellOrderId, int buyOrderId)
        {
            SellOrderId = sellOrderId;
            BuyOrderId = buyOrderId;
        }
    }
    
}