using Binance.Open.API.Net.Infrastructure;
using Binance.Open.API.Net.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace Binance.Open.API.Net.Services
{
    public partial class APIService : IAPIService
    {
        private readonly BinanceAPI _binanceAPI = new();
        private readonly string _serviceBaseUrl;

        public APIService()
        {
            _serviceBaseUrl = $"{_binanceAPI.BaseApiUrl}";
        }

        #region Non-Secure endpoints
        public virtual async Task<MarketTicker> MarketTickerAsync(string symbol)
        {
            var url = Endpoints.Market.Ticker(_serviceBaseUrl);
            var client = new RestClient(url)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddParameter("symbol", symbol, ParameterType.QueryString);
            var response = await client.ExecuteAsync(request);
            var responseContent = JsonConvert.DeserializeObject<MarketTicker>(response.Content);
            return responseContent;
        }
        #endregion
    }
}
