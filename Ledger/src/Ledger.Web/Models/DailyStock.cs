using System;
using System.ComponentModel.DataAnnotations;

namespace Ledger.Ledger.Web.Models
{
    public class DailyStock
    {
        [Key]
        public DateTime Date { get; set; }
        public Double StockValue{ get; set; }

        public DailyStock()
        {
            
        }

        public DailyStock(DateTime date, double stockValue)
        {
            Date = date;
            StockValue = stockValue;
        }
    }
}