using Binance.Open.API.Net.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Binance.Open.API.Net.Services
{
    public partial interface IAPIService
    {
        #region Non-Secure endpoints
        Task<MarketTicker> MarketTickerAsync(string symbol);
        Task<List<MarketTrades>> MarketTradesAsync(string symbol);
        #endregion
    }
}
