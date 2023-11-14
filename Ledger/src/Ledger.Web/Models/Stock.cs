using System;
using System.ComponentModel.DataAnnotations;

namespace Ledger.Ledger.Web.Models
{
    public class Stock // Stock is the main subject of the trading operations, it refers to the stock of a company and so its StockId, StockName fields are unique.
                       // It also has OpenDate, InitialStock, InitialPrice, CurrentStock, and CurrentPrice fields. InitialStock and InitialPrice are determined when
                       // the stock opens to the market and CurrentStock and CurrentPrice values are updated according to the transactions (buy/sell) operations in the market.

    {
    [Key] public int? StockId { get; set; } // randomly generated integer value
    public string StockName { get; set; }
    public DateTime OpenDate { get; set; }
    public int InitialStock { get; set; }
    public double InitialPrice { get; set; }
    public int CurrentStock { get; set; }
    public double CurrentPrice { get; set; }

    public Stock()
    {

    }

    public Stock(int stockId, string stockName, DateTime openDate, int initialStock, double initialPrice,
        int currentStock, double currentPrice)
    {
        StockId = stockId;
        StockName = stockName;
        OpenDate = openDate;
        InitialStock = initialStock;
        InitialPrice = initialPrice;
        CurrentStock = currentStock;
        CurrentPrice = currentPrice;
    }
    }
}