using System.Collections.Generic;

namespace Binance.Open.API.Net.Models
{
    public class MarketTrades
    {
        public List<Data> Data { get; set; }
    }
    public class Data
    {
        public int id { get; set; }
        public string price { get; set; }
        public string qty { get; set; }
        public string quoteQty { get; set; }
        public long time { get; set; }
        public bool isBuyerMaker { get; set; }
        public bool isBestMatch { get; set; }
    }
}
