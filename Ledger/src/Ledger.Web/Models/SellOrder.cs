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
        [Description("Matched, will be operated")]
        IsMatched = 4,
        [Description("Completed and deleted")]
        CompletedAndDeleted = 5,
        [Description("Partially completed and deleted")]
        PartiallyCompletedAndDeleted = 6,
        [Description("Not completed and deleted")]
        NotCompletedAndDeleted = 7
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
        
        public int CurrentAskSize { get; set; } //current available askSize, if it is 0 then the sellOrder is deleted 
        public  DateTime StartDate { get; set; } = DateTime.Now.ToUniversalTime(); //sell order is active from startdate to end of the day

        public OrderStatus Status { get; set; } = OrderStatus.Active; // ture: not deleted, false: deleted
        
        //[ForeignKey("UserId")]
        //public User User { get; set; } //navigation property
        
        //[ForeignKey("StockId")]
        //public Stock Stock { get; set; }

        public SellOrder()
        {
            
        }

        public SellOrder(int sellOrderId, int userId, int stockId, double askPrice, int askSize)
        {
            SellOrderId = sellOrderId;
            UserId = userId;
            StockId = stockId;
            AskPrice = askPrice;
            AskSize = askSize;
            CurrentAskSize = askSize;
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
                   $" StartDate: {StartDate}, Status: {Status}";
        }

    }
}