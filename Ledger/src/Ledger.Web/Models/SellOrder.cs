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
        public  DateTime StartDate { get; set; } = DateTime.Now; // default current time,sell order is active from startdate to enddate
        public  DateTime EndDate { get; set; } = DateTime.Now.AddDays(1); // default 1 day from startdate

        public bool Status { get; set; } = true; // ture: not deleted, false: deleted
        public bool IsActive { get; set; } = true; // true: active, false: not active (out of working times)
        
        
        //[ForeignKey("UserId")]
        //public User User { get; set; } //navigation property
        
        //[ForeignKey("StockId")]
        //public Stock Stock { get; set; }

        public SellOrder()
        {
            
        }

        public SellOrder(int? sellOrderId, int? userId, int? stockId, double askPrice, int askSize, bool status, bool isActive)
        {
            SellOrderId = sellOrderId;
            UserId = userId;
            StockId = stockId;
            AskPrice = askPrice;
            AskSize = askSize;
            Status = status;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return $"SellOrderId: {SellOrderId}, UserId: {UserId}, StockId: {StockId}, AskPrice: {AskPrice}, AskSize: {AskSize}, " +
                   $"DateCreated: {DateCreated}, StartDate: {StartDate}, EndDate: {EndDate}, Status: {Status}";
        }

    }
}