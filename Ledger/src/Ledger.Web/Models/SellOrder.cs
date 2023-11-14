using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledger.Ledger.Web.Models
{
    public class SellOrder // This class refers to the object which stores relevant information about a sell order. It has SellOrderId (primary key),
                           // UserId and StockId (Foreign Keys), AskPrice, AskSize, DateCreated fields.
    {
        [Key]
        public int? SellOrderId { get; set; } // randomly generated integer
        public  int? UserId { get; set; }
        public  int? StockId { get; set; }
        public  double AskPrice { get; set; }
        public  int AskSize { get; set; }
        public  DateTime DateCreated { get; set; } = DateTime.Now;
        
        [ForeignKey("UserId")]
        public User User { get; set; } //navigation property
        
        [ForeignKey("StockId")]
        public Stock Stock { get; set; }

        public SellOrder()
        {
            
        }

        public SellOrder(int sellOrderId, int userId, int stockId, double askPrice, int askSize, DateTime dateCreated)
        {
            SellOrderId = sellOrderId;
            UserId = userId;
            StockId = stockId;
            AskPrice = askPrice;
            AskSize = askSize;
            DateCreated = dateCreated;
        }
    }
}