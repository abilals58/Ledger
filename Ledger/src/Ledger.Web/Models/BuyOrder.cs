using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Ledger.Ledger.Web.Models
{
    public class BuyOrder    // This class refers to the object which stores relevant information about a buy order. It has BuyOrderId (primary key),
                            // UserId and StockId (Foreign Keys), BidPrice, BidSize, DateCreated fields.
    {
        [Key]
        public int BuyOrderId { get; set; } // randomly generated integer
        public int UserId { get; set; }
        public  int StockId { get; set; }
        public  double BidPrice { get; set; }
        public  int BidSize { get; set; }
        public  DateTime DateCreated { get; set; } = DateTime.Now;
        public  DateTime StartDate { get; set; } = DateTime.Now; // default current time,buy order is active from startdate to enddate
        public  DateTime EndDate { get; set; } = DateTime.Now.AddDays(1); // default 1 day from startdate
        
        public bool Status { get; set; } = true; // ture: not deleted, false: deleted
        //public bool IsActive { get; set; } = true; // true: active, false: not active (out of working times)
        
        //public List<SellOrder> MatchList { get; set; } = new();
        
        
        //[ForeignKey("UserId")]
        //public User User { get; set; } //navigation property
        
        //[ForeignKey("StockId")]
        //public Stock Stock { get; set; }
        
        public BuyOrder()
        {
            
        }

        public BuyOrder(int buyOrderId, int userId, int stockId, double bidPrice, int bidSize, bool status)
        {
            BuyOrderId = buyOrderId;
            UserId = userId;
            StockId = stockId;
            BidPrice = bidPrice;
            BidSize = bidSize;
            Status = status;
        }

        public override string ToString()
        {
            return $"BuyOrderId: {BuyOrderId}, UserId: {UserId}, StockId: {StockId}, BidPrice: {BidPrice}, BidSize: {BidSize}, " +
                   $"DateCreated: {DateCreated}, StartDate: {StartDate}, EndDate: {EndDate}, Status: {Status}";
        }
    }
}