using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledger.Ledger.Web.Models
{
    public class BuyOrder    // This class refers to the object which stores relevant information about a buy order. It has BuyOrderId (primary key),
                            // UserId and StockId (Foreign Keys), BidPrice, BidSize, DateCreated fields.
    {
        [Key]
        public int? BuyOrderId { get; set; } // randomly generated integer
        public int? UserId { get; set; }
        public  int? StockId { get; set; }
        public  double BidPrice { get; set; }
        public  int BidSize { get; set; }
        public  DateTime DateCreated { get; set; } = DateTime.Now;
        
        [ForeignKey("UserId")]
        public User User { get; set; } //navigation property
        
        [ForeignKey("StockId")]
        public Stock Stock { get; set; }
        
        public BuyOrder()
        {
            
        }

        public BuyOrder(int buyOrderId, int userId, int stockId, double bidPrice, int bidSize, DateTime dateCreated)
        {
            BuyOrderId = buyOrderId;
            UserId = userId;
            StockId = stockId;
            BidPrice = bidPrice;
            BidSize = bidSize;
            DateCreated = dateCreated;
        }
    }
}