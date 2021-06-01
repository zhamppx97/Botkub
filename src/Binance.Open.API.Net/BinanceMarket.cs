using Binance.Open.API.Net.Models;
using Binance.Open.API.Net.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Binance.Open.API.Net
{
    public partial class BinanceMarket : IBinanceMarket
    {
        private readonly IAPIService _apiService = new APIService();

        public virtual async Task<MarketTicker> GetTickerAsync(string symbol)
        {
            return await _apiService.MarketTickerAsync(symbol);
        }
        public virtual async Task<List<MarketTrades>> GetTradesAsync(string symbol)
        {
            return await _apiService.MarketTradesAsync(symbol);
        }
    }
}
