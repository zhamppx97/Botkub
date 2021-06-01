using Binance.Open.API.Net.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Binance.Open.API.Net
{
    public partial interface IBinanceMarket
    {
        Task<MarketTicker> GetTickerAsync(string symbol);
        Task<List<MarketTrades>> GetTradesAsync(string symbol);
    }
}
