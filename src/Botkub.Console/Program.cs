using Binance.Open.API.Net;
using Bitkub.Open.API.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Botkub.ConsoleApp
{
    class Program
    {
        private class Coins
        {
            public string CoinName { get; set; }
            public double QuoteVolume { get; set; }
            public double PercentChange { get; set; }
        }
        static void Main(string[] args)
        {
            int timeHr = int.Parse(ConfigurationManager.AppSettings["TimeHR"]);
            Timer bitkubMarketTicker = new(BitkubTimerMarketTickerCallback, null, 0, 1000 * 60 * 60 * timeHr);
            Timer bitkubWatchList = new(BitkubWatchList, null, 0, 1000 * 30);
            Timer binanceMarketTicker = new(BinanceTimerMarketTickerCallback, null, 0, 1000 * 60 * 60 * timeHr);
            Console.ReadLine();
        }
        static void SendLineNotify(string message)
        {
            if (!message.Equals(""))
            {
                string token = "LINE-notify-token";
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", message);
                var data = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);
                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }
        #region Bitkub
        static async Task BitkubMarketTickerAsync()
        {
            try
            {
                IBitkubMarket market = new BitkubMarket();
                string sym = null; // ex THB_BTC
                var result = await market.GetTickerAsync(sym);
                IList<Coins> listCoins = new List<Coins>()
                {
                    new Coins{ CoinName = "BTC", QuoteVolume = Convert.ToDouble(result.THB_BTC.quoteVolume), PercentChange = Convert.ToDouble(result.THB_BTC.percentChange) },
                    new Coins{ CoinName = "ETH", QuoteVolume = Convert.ToDouble(result.THB_ETH.quoteVolume), PercentChange = Convert.ToDouble(result.THB_ETH.percentChange) },
                    new Coins{ CoinName = "XRP", QuoteVolume = Convert.ToDouble(result.THB_XRP.quoteVolume), PercentChange = Convert.ToDouble(result.THB_XRP.percentChange) },
                    new Coins{ CoinName = "LTC", QuoteVolume = Convert.ToDouble(result.THB_LTC.quoteVolume), PercentChange = Convert.ToDouble(result.THB_LTC.percentChange) },
                    new Coins{ CoinName = "BCH", QuoteVolume = Convert.ToDouble(result.THB_BCH.quoteVolume), PercentChange = Convert.ToDouble(result.THB_BCH.percentChange) },
                    new Coins{ CoinName = "USDT", QuoteVolume = Convert.ToDouble(result.THB_USDT.quoteVolume), PercentChange = Convert.ToDouble(result.THB_USDT.percentChange) },
                    new Coins{ CoinName = "BNB", QuoteVolume = Convert.ToDouble(result.THB_BNB.quoteVolume), PercentChange = Convert.ToDouble(result.THB_BNB.percentChange) },
                    new Coins{ CoinName = "XLM", QuoteVolume = Convert.ToDouble(result.THB_XLM.quoteVolume), PercentChange = Convert.ToDouble(result.THB_XLM.percentChange) },
                    new Coins{ CoinName = "ADA", QuoteVolume = Convert.ToDouble(result.THB_ADA.quoteVolume), PercentChange = Convert.ToDouble(result.THB_ADA.percentChange) },
                    new Coins{ CoinName = "BSV", QuoteVolume = Convert.ToDouble(result.THB_BSV.quoteVolume), PercentChange = Convert.ToDouble(result.THB_BSV.percentChange) },
                    new Coins{ CoinName = "WAN", QuoteVolume = Convert.ToDouble(result.THB_WAN.quoteVolume), PercentChange = Convert.ToDouble(result.THB_WAN.percentChange) },
                    new Coins{ CoinName = "OMG", QuoteVolume = Convert.ToDouble(result.THB_OMG.quoteVolume), PercentChange = Convert.ToDouble(result.THB_OMG.percentChange) },
                    new Coins{ CoinName = "ZIL", QuoteVolume = Convert.ToDouble(result.THB_ZIL.quoteVolume), PercentChange = Convert.ToDouble(result.THB_ZIL.percentChange) },
                    new Coins{ CoinName = "SNT", QuoteVolume = Convert.ToDouble(result.THB_SNT.quoteVolume), PercentChange = Convert.ToDouble(result.THB_SNT.percentChange) },
                    new Coins{ CoinName = "CVC", QuoteVolume = Convert.ToDouble(result.THB_CVC.quoteVolume), PercentChange = Convert.ToDouble(result.THB_CVC.percentChange) },
                    new Coins{ CoinName = "LINK", QuoteVolume = Convert.ToDouble(result.THB_LINK.quoteVolume), PercentChange = Convert.ToDouble(result.THB_LINK.percentChange) },
                    new Coins{ CoinName = "IOST", QuoteVolume = Convert.ToDouble(result.THB_IOST.quoteVolume), PercentChange = Convert.ToDouble(result.THB_IOST.percentChange) },
                    new Coins{ CoinName = "ZRX", QuoteVolume = Convert.ToDouble(result.THB_ZRX.quoteVolume), PercentChange = Convert.ToDouble(result.THB_ZRX.percentChange) },
                    new Coins{ CoinName = "KNC", QuoteVolume = Convert.ToDouble(result.THB_KNC.quoteVolume), PercentChange = Convert.ToDouble(result.THB_KNC.percentChange) },
                    new Coins{ CoinName = "RDN", QuoteVolume = Convert.ToDouble(result.THB_RDN.quoteVolume), PercentChange = Convert.ToDouble(result.THB_RDN.percentChange) },
                    new Coins{ CoinName = "ABT", QuoteVolume = Convert.ToDouble(result.THB_ABT.quoteVolume), PercentChange = Convert.ToDouble(result.THB_ABT.percentChange) },
                    new Coins{ CoinName = "MANA", QuoteVolume = Convert.ToDouble(result.THB_MANA.quoteVolume), PercentChange = Convert.ToDouble(result.THB_MANA.percentChange) },
                    //new CoinPercentChange{ CoinName = "CTXC", PercentChange = Convert.ToDouble(result.THB_CTXC.percentChange) },
                    new Coins{ CoinName = "SIX", QuoteVolume = Convert.ToDouble(result.THB_SIX.quoteVolume), PercentChange = Convert.ToDouble(result.THB_SIX.percentChange) },
                    new Coins{ CoinName = "JFIN", QuoteVolume = Convert.ToDouble(result.THB_JFIN.quoteVolume), PercentChange = Convert.ToDouble(result.THB_JFIN.percentChange) },
                    new Coins{ CoinName = "EVX", QuoteVolume = Convert.ToDouble(result.THB_EVX.quoteVolume), PercentChange = Convert.ToDouble(result.THB_EVX.percentChange) },
                    new Coins{ CoinName = "POW", QuoteVolume = Convert.ToDouble(result.THB_POW.quoteVolume), PercentChange = Convert.ToDouble(result.THB_POW.percentChange) },
                    new Coins{ CoinName = "DOGE", QuoteVolume = Convert.ToDouble(result.THB_DOGE.quoteVolume), PercentChange = Convert.ToDouble(result.THB_DOGE.percentChange) },
                    new Coins{ CoinName = "DAI", QuoteVolume = Convert.ToDouble(result.THB_DAI.quoteVolume), PercentChange = Convert.ToDouble(result.THB_DAI.percentChange) },
                    new Coins{ CoinName = "USDC", QuoteVolume = Convert.ToDouble(result.THB_USDC.quoteVolume), PercentChange = Convert.ToDouble(result.THB_USDC.percentChange) },
                    new Coins{ CoinName = "BAT", QuoteVolume = Convert.ToDouble(result.THB_BAT.quoteVolume), PercentChange = Convert.ToDouble(result.THB_BAT.percentChange) },
                    new Coins{ CoinName = "MKR", QuoteVolume = Convert.ToDouble(result.THB_MKR.quoteVolume), PercentChange = Convert.ToDouble(result.THB_MKR.percentChange) },
                    new Coins{ CoinName = "ENJ", QuoteVolume = Convert.ToDouble(result.THB_ENJ.quoteVolume), PercentChange = Convert.ToDouble(result.THB_ENJ.percentChange) },
                    new Coins{ CoinName = "BAND", QuoteVolume = Convert.ToDouble(result.THB_BAND.quoteVolume), PercentChange = Convert.ToDouble(result.THB_BAND.percentChange) },
                    new Coins{ CoinName = "COMP", QuoteVolume = Convert.ToDouble(result.THB_COMP.quoteVolume), PercentChange = Convert.ToDouble(result.THB_COMP.percentChange) },
                    new Coins{ CoinName = "KSM", QuoteVolume = Convert.ToDouble(result.THB_KSM.quoteVolume), PercentChange = Convert.ToDouble(result.THB_KSM.percentChange) },
                    new Coins{ CoinName = "DOT", QuoteVolume = Convert.ToDouble(result.THB_DOT.quoteVolume), PercentChange = Convert.ToDouble(result.THB_DOT.percentChange) },
                    new Coins{ CoinName = "NEAR", QuoteVolume = Convert.ToDouble(result.THB_NEAR.quoteVolume), PercentChange = Convert.ToDouble(result.THB_NEAR.percentChange) },
                    new Coins{ CoinName = "SCRT", QuoteVolume = Convert.ToDouble(result.THB_SCRT.quoteVolume), PercentChange = Convert.ToDouble(result.THB_SCRT.percentChange) },
                    new Coins{ CoinName = "GLM", QuoteVolume = Convert.ToDouble(result.THB_GLM.quoteVolume), PercentChange = Convert.ToDouble(result.THB_GLM.percentChange) },
                    new Coins{ CoinName = "YFI", QuoteVolume = Convert.ToDouble(result.THB_YFI.quoteVolume), PercentChange = Convert.ToDouble(result.THB_YFI.percentChange) },
                    new Coins{ CoinName = "UNI", QuoteVolume = Convert.ToDouble(result.THB_UNI.quoteVolume), PercentChange = Convert.ToDouble(result.THB_UNI.percentChange) },
                    new Coins{ CoinName = "AAVE", QuoteVolume = Convert.ToDouble(result.THB_AAVE.quoteVolume), PercentChange = Convert.ToDouble(result.THB_AAVE.percentChange) },
                    new Coins{ CoinName = "ALPHA", QuoteVolume = Convert.ToDouble(result.THB_ALPHA.quoteVolume), PercentChange = Convert.ToDouble(result.THB_ALPHA.percentChange) },
                    new Coins{ CoinName = "CRV", QuoteVolume = Convert.ToDouble(result.THB_CRV.quoteVolume), PercentChange = Convert.ToDouble(result.THB_CRV.percentChange) },
                };

                string[] watchList = ConfigurationManager.AppSettings["BitkubWatchList"].Split(',');
                foreach (var coinName in watchList)
                {
                    string symbol = "THB_" + coinName;
                    int limit = 10000;
                    var trades = await market.GetTradesAsync(symbol, limit);
                    int buyCount = 0;
                    int sellCount = 0;
                    foreach (var item in trades.result)
                    {
                        JArray itemArray = JArray.Parse(item.ToString());
                        if (itemArray.Last.ToString().Equals("BUY"))
                        {
                            buyCount += 1;
                        }
                        if (itemArray.Last.ToString().Equals("SELL"))
                        {
                            sellCount += 1;
                        }
                    }

                    var watchCoin = result.GetType().GetProperty(symbol).GetValue(result, null);
                    StringBuilder sbTicker = new();
                    sbTicker.Append("Good day Boss!" + Environment.NewLine);
                    sbTicker.Append("🚀🚀🚀🎉🎉🎉" + Environment.NewLine);
                    sbTicker.Append("💚 Bitkub Market Ticker 💚" + Environment.NewLine);
                    sbTicker.Append("💚[Bitkub] #" + coinName + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(watchCoin.GetType().GetProperty("last").GetValue(watchCoin, null))));
                    sbTicker.AppendFormat("High 24 hrs: {0}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(watchCoin.GetType().GetProperty("high24hr").GetValue(watchCoin, null))));
                    sbTicker.AppendFormat("Low 24 hrs: {0}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(watchCoin.GetType().GetProperty("low24hr").GetValue(watchCoin, null))));
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, watchCoin.GetType().GetProperty("percentChange").GetValue(watchCoin, null));
                    sbTicker.AppendFormat("Trades count: {0} | BUY = {1}, SELL = {2}" + Environment.NewLine, limit.ToString(), buyCount, sellCount );
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);

                    Console.WriteLine(sbTicker);
                    SendLineNotify(sbTicker.ToString());
                }

                StringBuilder sbTopGainer = new();
                var filterTopGainer = listCoins.OrderByDescending(o => o.PercentChange).Take(10);
                if (filterTopGainer != null)
                {
                    sbTopGainer.Append("💚[Bitkub] Top 10 Gainer" + Environment.NewLine);
                    sbTopGainer.Append("24 hrs Percent Change" + Environment.NewLine);
                    sbTopGainer.Append("🚀🚀🚀🎉🎉🎉" + Environment.NewLine);
                    foreach (var item in filterTopGainer)
                    {
                        sbTopGainer.AppendFormat("#{0} : {1}" + Environment.NewLine, item.CoinName, item.PercentChange);
                    }
                }

                StringBuilder sbPopQuote = new();
                var filterPopQoute = listCoins.OrderByDescending(o => o.QuoteVolume).Take(10);
                if (filterPopQoute != null)
                {
                    sbPopQuote.Append("💚[Bitkub] Top 10 Popular qoute" + Environment.NewLine);
                    sbPopQuote.Append("Volume(THB)" + Environment.NewLine);
                    sbPopQuote.Append("🚀🚀🚀🎉🎉🎉" + Environment.NewLine);
                    foreach (var item in filterPopQoute)
                    {
                        sbPopQuote.AppendFormat("#{0} : {1}฿" + Environment.NewLine, item.CoinName, string.Format("{0:#,0.####}", item.QuoteVolume));
                    }
                }

                if (!sbTopGainer.ToString().Equals(""))
                {
                    Console.WriteLine(sbTopGainer);
                    SendLineNotify(sbTopGainer.ToString());
                }
                if (!sbPopQuote.ToString().Equals(""))
                {
                    Console.WriteLine(sbPopQuote);
                    SendLineNotify(sbPopQuote.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void BitkubTimerMarketTickerCallback(Object o)
        {
            Console.WriteLine("[Bitkub] Market ticker execute time: " + DateTime.Now);
            BitkubMarketTickerAsync().Wait();
            GC.Collect();
        }
        static async Task BitkubWatchListWorkerAsync()
        {
            try
            {
                string[] watchList = ConfigurationManager.AppSettings["BitkubWatchList"].Split(',');
                foreach (var coinName in watchList)
                {
                    Console.WriteLine("[Bitkub] #" + coinName + " Watch list execute time: " + DateTime.Now);

                    IBitkubMarket market = new BitkubMarket();
                    string sym = "THB_" + coinName;
                    int limit = 30;
                    var trades = await market.GetTradesAsync(sym, limit);
                    foreach (var value in trades.result)
                    {
                        string message = "";
                        JArray itemArray = JArray.Parse(value.ToString());

                        Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        DateTime tradeDatetime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        tradeDatetime = tradeDatetime.AddSeconds((double)itemArray[0]).ToLocalTime();

                        if (itemArray.Last.ToString().Equals("BUY"))
                        {
                            double priceTHB = (double)itemArray[1];
                            double amount = (double)itemArray[2];
                            // Shark
                            if (amount >= 500 && amount <= 1000)
                            {
                                message = "💚[Bitkub] #" + coinName + Environment.NewLine + " 🦈 Shark traded! #BUY " + amount.ToString() + "#" + coinName + " | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                            }
                            // Whale
                            else if (amount >= 1000 && amount <= 5000)
                            {
                                message = "💚[Bitkub] #" + coinName + Environment.NewLine + " 🐋 Whale traded! #BUY " + amount.ToString() + "#" + coinName + " | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                            }
                            // Humpback Whale
                            else if (amount >= 5000)
                            {
                                message = "💚[Bitkub] #" + coinName + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #BUY " + amount.ToString() + "#" + coinName + " | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                            }
                        }
                        if (itemArray.Last.ToString().Equals("SELL"))
                        {
                            double priceTHB = (double)itemArray[1];
                            double amount = (double)itemArray[2];
                            // Shark
                            if (amount >= 500 && amount <= 1000)
                            {
                                message = "💚[Bitkub] #" + coinName + Environment.NewLine + " 🦈 Shark traded! #SELL " + amount.ToString() + "#" + coinName + " | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                            }
                            // Whale
                            else if (amount >= 1000 && amount <= 5000)
                            {
                                message = "💚[Bitkub] #" + coinName + Environment.NewLine + " 🐋 Whale traded! #SELL " + amount.ToString() + "#" + coinName + " | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                            }
                            // Humpback Whale
                            else if (amount >= 5000)
                            {
                                message = "💚[Bitkub] #" + coinName + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #SELL " + amount.ToString() + "#" + coinName + " | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                            }
                        }

                        if (!message.Equals(""))
                        {
                            Console.WriteLine(message);
                            SendLineNotify(message);
                        }
                    }

                    StringBuilder sbTicker = new();
                    var ticker = await market.GetTickerAsync(sym);
                    var tickerCoin = ticker.GetType().GetProperty(sym).GetValue(ticker, null);
                    if (Convert.ToDouble(tickerCoin.GetType().GetProperty("percentChange").GetValue(tickerCoin, null)) <= (-10))
                    {
                        sbTicker.Append("💚[Bitkub] #" + coinName + " 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.Append("Percent change is too low! 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(tickerCoin.GetType().GetProperty("last").GetValue(tickerCoin, null))));
                        sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(tickerCoin.GetType().GetProperty("high24hr").GetValue(tickerCoin, null))));
                        sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(tickerCoin.GetType().GetProperty("low24hr").GetValue(tickerCoin, null))));
                        sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, tickerCoin.GetType().GetProperty("percentChange").GetValue(tickerCoin, null));
                        sbTicker.Append("-------------------------------------" + Environment.NewLine);

                        Console.WriteLine(sbTicker);
                        SendLineNotify(sbTicker.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void BitkubWatchList(Object o)
        {
            Console.WriteLine("[Bitkub] Watch list execute time: " + DateTime.Now);
            BitkubWatchListWorkerAsync().Wait();
            GC.Collect();
        }
        #endregion
        #region Binance
        static async Task BinanceMarketTickerAsync()
        {
            try
            {
                IBinanceMarket marketBinance = new BinanceMarket();
                IBitkubMarket marketBitkub = new BitkubMarket();
                string sym = "THB_USDT";
                var resultTHBUSDT = await marketBitkub.GetTickerAsync(sym);

                string symBTCUSDT = "BTCUSDT";
                var resultBTCUSDT = await marketBinance.GetTickerAsync(symBTCUSDT);
                string symETHUSDT = "ETHUSDT";
                var resultETHUSDT = await marketBinance.GetTickerAsync(symETHUSDT);
                string symBNBUSDT = "BNBUSDT";
                var resultBNBUSDT = await marketBinance.GetTickerAsync(symBNBUSDT);
                string symKSMUSDT = "KSMUSDT";
                var resultKSMUSDT = await marketBinance.GetTickerAsync(symKSMUSDT);

                StringBuilder sbTicker = new();
                sbTicker.Append("Good day Boss!" + Environment.NewLine);
                sbTicker.Append("🚀🚀🚀🎉🎉🎉" + Environment.NewLine);
                sbTicker.Append("🔶 Binance Market Ticker 🔶" + Environment.NewLine);
                if (resultBTCUSDT != null)
                {
                    string BTCUSDTlastPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultBTCUSDT.lastPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string BTCUSDThighPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultBTCUSDT.highPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string BTCUSDTlowPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultBTCUSDT.lowPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));

                    sbTicker.Append("🔶[Binance] Bitcoin #BTC : USDT" + Environment.NewLine);
                    sbTicker.Append("#BTCUSDT x #THBUSDT" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultBTCUSDT.lastPrice)), BTCUSDTlastPriceToBaht);
                    sbTicker.AppendFormat("High 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultBTCUSDT.highPrice)), BTCUSDThighPriceToBaht);
                    sbTicker.AppendFormat("Low 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultBTCUSDT.lowPrice)), BTCUSDTlowPriceToBaht);
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultBTCUSDT.priceChangePercent);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }
                if (resultETHUSDT != null)
                {
                    string ETHUSDTlastPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultETHUSDT.lastPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string ETHUSDThighPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultETHUSDT.highPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string ETHUSDTlowPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultETHUSDT.lowPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));

                    sbTicker.Append("🔶[Binance] Ethereum #ETH : USDT" + Environment.NewLine);
                    sbTicker.Append("#ETHUSDT x #THBUSDT" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultETHUSDT.lastPrice)), ETHUSDTlastPriceToBaht);
                    sbTicker.AppendFormat("High 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultETHUSDT.highPrice)), ETHUSDThighPriceToBaht);
                    sbTicker.AppendFormat("Low 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultETHUSDT.lowPrice)), ETHUSDTlowPriceToBaht);
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultETHUSDT.priceChangePercent);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }
                if (resultBNBUSDT != null)
                {
                    string BNBUSDTlastPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultBNBUSDT.lastPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string BNBUSDThighPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultBNBUSDT.highPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string BNBUSDTlowPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultBNBUSDT.lowPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));

                    sbTicker.Append("🔶[Binance] Binance #BNB : USDT" + Environment.NewLine);
                    sbTicker.Append("#BNBUSDT x #THBUSDT" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultBNBUSDT.lastPrice)), BNBUSDTlastPriceToBaht);
                    sbTicker.AppendFormat("High 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultBNBUSDT.highPrice)), BNBUSDThighPriceToBaht);
                    sbTicker.AppendFormat("Low 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultBNBUSDT.lowPrice)), BNBUSDTlowPriceToBaht);
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultBNBUSDT.priceChangePercent);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }
                if (resultKSMUSDT != null)
                {
                    string KSMUSDTlastPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultKSMUSDT.lastPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string KSMUSDThighPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultKSMUSDT.highPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));
                    string KSMUSDTlowPriceToBaht = string.Format("{0:#,0.####}", Convert.ToDouble(resultKSMUSDT.lowPrice) * Convert.ToDouble(resultTHBUSDT.THB_USDT.last));

                    sbTicker.Append("🔶[Binance] Kusama #KSM : USDT" + Environment.NewLine);
                    sbTicker.Append("#KSMUSDT x #THBUSDT" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultKSMUSDT.lastPrice)), KSMUSDTlastPriceToBaht);
                    sbTicker.AppendFormat("High 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultKSMUSDT.highPrice)), KSMUSDThighPriceToBaht);
                    sbTicker.AppendFormat("Low 24 hrs: {0} ≈ {1}฿" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultKSMUSDT.lowPrice)), KSMUSDTlowPriceToBaht);
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultKSMUSDT.priceChangePercent);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }

                if (!sbTicker.ToString().Equals(""))
                {
                    Console.WriteLine(sbTicker);
                    SendLineNotify(sbTicker.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void BinanceTimerMarketTickerCallback(Object o)
        {
            Console.WriteLine("[Binance] Market ticker execute time: " + DateTime.Now);
            BinanceMarketTickerAsync().Wait();
            GC.Collect();
        }
        #endregion
    }
}
