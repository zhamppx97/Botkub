
namespace Binance.Open.API.Net.Infrastructure
{
    public static class Endpoints
    {
        public static class Market
        {
            #region Non-Secure endpoints
            public static string Ticker(string baseUrl)
            {
                return $"{baseUrl}/api/v3/ticker/24hr";
            }
            #endregion
        }
    }
}
