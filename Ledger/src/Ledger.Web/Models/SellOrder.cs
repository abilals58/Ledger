using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ledger.Ledger.Web.Models
{
    public enum OrderStatus{
        [Description("Active")]
        Active=1,
        [Description("Partially completed and active")]
        PartiallyCompletedAndActive=2,
        [Description("Not Active, will activated on the beginning of the next day")]
        NotYetActive = 3,
        [Description("Completed and deleted")]
        CompletedAndDeleted = 4,
        [Description("Partially completed and deleted")]
        PartiallyCompletedAndDeleted = 5,
        [Description("Not completed and deleted")]
        NotCompletedAndDeleted = 6
    }
    
    public class SellOrder // This class refers to the object which stores relevant information about a sell order. It has SellOrderId (primary key),
                           // UserId and StockId (Foreign Keys), AskPrice, AskSize, DateCreated fields.
    {
        [Key]
        public int SellOrderId { get; set; } // randomly generated integer
        public  int UserId { get; set; }
        public  int StockId { get; set; }
        public  double AskPrice { get; set; }
        public  int AskSize { get; set; }
        public  DateTime DateCreated { get; set; } = DateTime.Now;
        public  DateTime StartDate { get; set; } = DateTime.Now; // default current time,sell order is active from startdate to enddate
        public  DateTime EndDate { get; set; } = DateTime.Now.AddDays(1); // default 1 day from startdate

        public OrderStatus Status { get; set; } = OrderStatus.Active; // ture: not deleted, false: deleted
        
        public bool IsMatched { get; set; } = false;
        //public bool IsActive { get; set; } = true; // true: active, false: not active (out of working times)
        
        
        //[ForeignKey("UserId")]
        //public User User { get; set; } //navigation property
        
        //[ForeignKey("StockId")]
        //public Stock Stock { get; set; }

        public SellOrder()
        {
            
        }

        public SellOrder(int sellOrderId, int userId, int stockId, double askPrice, int askSize, bool isMatched)
        {
            SellOrderId = sellOrderId;
            UserId = userId;
            StockId = stockId;
            AskPrice = askPrice;
            AskSize = askSize;
            IsMatched = isMatched;
            //check whether in working hours or not 
            if (DateTime.Now.Hour > 9 && DateTime.Now.Hour < 17)
            {
                Status = OrderStatus.Active;
            }
            else
            {
                Status = OrderStatus.NotYetActive;
            }
        }

        public override string ToString()
        {
            return $"SellOrderId: {SellOrderId}, UserId: {UserId}, StockId: {StockId}, AskPrice: {AskPrice}, AskSize: {AskSize}, " +
                   $"DateCreated: {DateCreated}, StartDate: {StartDate}, EndDate: {EndDate}, Status: {Status}";
        }

    }
}