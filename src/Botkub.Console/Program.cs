using Bitkub.Open.API.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            Timer marketTicker = new(TimerMarketTickerCallback, null, 0, 1000 * 60 * 60 * 2);
            Timer tradesBTC = new(TimerTradesBTCCallback, null, 0, 1000 * 30);
            Timer tradesETH = new(TimerTradesETHCallback, null, 0, 1000 * 30);
            Timer tradesBNB = new(TimerTradesBNBCallback, null, 0, 1000 * 30);
            //Timer tradesKSM = new(TimerTradesKSMCallback, null, 0, 1000 * 30);
            Timer tradesCOMP = new(TimerTradesCOMPCallback, null, 0, 1000 * 30);
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

        static async Task MarketTickerAsync()
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
                };

                StringBuilder sbTicker = new();
                sbTicker.Append("Good day Boss!" + Environment.NewLine);
                sbTicker.Append("🚀🚀🚀🎉🎉🎉" + Environment.NewLine);
                if (result.THB_BTC != null)
                {
                    sbTicker.Append("[Bitkub] Bitcoin #BTC" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BTC.last)));
                    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BTC.high24hr)));
                    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BTC.low24hr)));
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_BTC.percentChange);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }
                if (result.THB_ETH != null)
                {
                    sbTicker.Append("[Bitkub] Ethereum #ETH" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_ETH.last)));
                    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_ETH.high24hr)));
                    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_ETH.low24hr)));
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_ETH.percentChange);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }
                //if (result.THB_XRP != null)
                //{
                //    sbTicker.Append("[Bitkub] XRP #XRP" + Environment.NewLine);
                //    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_XRP.last)));
                //    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_XRP.high24hr)));
                //    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_XRP.low24hr)));
                //    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_XRP.percentChange);
                //    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                //}
                //if (result.THB_LTC != null)
                //{
                //    sbTicker.Append("[Bitkub] Litecoin #LTC" + Environment.NewLine);
                //    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_LTC.last)));
                //    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_LTC.high24hr)));
                //    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_LTC.low24hr)));
                //    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_LTC.percentChange);
                //    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                //}
                //if (result.THB_BCH != null)
                //{
                //    sbTicker.Append("[Bitkub] Bitcoin Cash #BCH" + Environment.NewLine);
                //    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BCH.last)));
                //    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BCH.high24hr)));
                //    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BCH.low24hr)));
                //    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_BCH.percentChange);
                //    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                //}
                //if (result.THB_USDT != null)
                //{
                //    sbTicker.Append("[Bitkub] USDT #USDT" + Environment.NewLine);
                //    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_USDT.last)));
                //    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_USDT.high24hr)));
                //    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_USDT.low24hr)));
                //    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_USDT.percentChange);
                //    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                //}
                //if (result.THB_KSM != null)
                //{
                //    sbTicker.Append("[Bitkub] Kusama #KSM" + Environment.NewLine);
                //    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_KSM.last)));
                //    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_KSM.high24hr)));
                //    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_KSM.low24hr)));
                //    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_KSM.percentChange);
                //    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                //}
                if (result.THB_BNB != null)
                {
                    sbTicker.Append("[Bitkub] Binance #BNB" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BNB.last)));
                    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BNB.high24hr)));
                    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_BNB.low24hr)));
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_BNB.percentChange);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }
                if (result.THB_COMP != null)
                {
                    sbTicker.Append("[Bitkub] Compound #COMP" + Environment.NewLine);
                    sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_COMP.last)));
                    sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_COMP.high24hr)));
                    sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(result.THB_COMP.low24hr)));
                    sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, result.THB_COMP.percentChange);
                    sbTicker.Append("-------------------------------------" + Environment.NewLine);
                }

                StringBuilder sbTopGainer = new();
                var filterTopGainer = listCoins.OrderByDescending(o => o.PercentChange).Take(10);
                if (filterTopGainer != null)
                {
                    sbTopGainer.Append("[Bitkub] Top 10 Gainer" + Environment.NewLine);
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
                    sbPopQuote.Append("[Bitkub] Top 10 Popular qoute" + Environment.NewLine);
                    sbPopQuote.Append("Volume(THB)" + Environment.NewLine);
                    sbPopQuote.Append("🚀🚀🚀🎉🎉🎉" + Environment.NewLine);
                    foreach (var item in filterPopQoute)
                    {
                        sbPopQuote.AppendFormat("#{0} : {1}" + Environment.NewLine, item.CoinName, string.Format("{0:#,0.####}", item.QuoteVolume));
                    }
                }

                if (!sbTicker.ToString().Equals(""))
                {
                    Console.WriteLine(sbTicker);
                    SendLineNotify(sbTicker.ToString());
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

        static void TimerMarketTickerCallback(Object o)
        {
            Console.WriteLine("[Bitkub] Market ticker execute time: " + DateTime.Now);
            MarketTickerAsync().Wait();
            GC.Collect();
        }

        static async Task TradesBTC()
        {
            try
            {
                IBitkubMarket market = new BitkubMarket();
                string sym = "THB_BTC";
                int limit = 30;
                var result = await market.GetTradesAsync(sym, limit);
                foreach (var value in result.result)
                {
                    string message = "";
                    JArray itemArray = JArray.Parse(value.ToString());

                    Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    DateTime tradeDatetime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    tradeDatetime = tradeDatetime.AddSeconds((double)itemArray[0]).ToLocalTime();

                    if (itemArray.Last.ToString().Equals("BUY"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double btc = (double)itemArray[2];
                        //// Crab
                        //if (btc >= 1 && btc <= 10)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🦀 Crab traded! #BUY " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        //// Octopus
                        //else if (btc >= 10 && btc <= 50)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐙 Octopus traded! #BUY " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        //// Fish
                        //else if (btc >= 50 && btc <= 100)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐟 Fish traded! #BUY " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        //// Dolphin
                        //else if (btc >= 100 && btc <= 500)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐬 Dolphin traded! #BUY " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        // Shark
                        if (btc >= 500 && btc <= 1000)
                        {
                            message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🦈 Shark traded! #BUY " + btc.ToString() + "#BTC | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (btc >= 1000 && btc <= 5000)
                        {
                            message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐋 Whale traded! #BUY " + btc.ToString() + "#BTC | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (btc >= 5000)
                        {
                            message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #BUY " + btc.ToString() + "#BTC | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }
                    if (itemArray.Last.ToString().Equals("SELL"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double btc = (double)itemArray[2];
                        //// Crab
                        //if (btc >= 1 && btc <= 10)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🦀 Crab traded! #SELL " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        //// Octopus
                        //else if (btc >= 10 && btc <= 50)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐙 Octopus traded! #SELL " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        //// Fish
                        //else if (btc >= 50 && btc <= 100)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐟 Fish traded! #SELL " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        //// Dolphin
                        //else if (btc >= 100 && btc <= 500)
                        //{
                        //    message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐬 Dolphin traded! #SELL " + btc.ToString() + " Date: " + tradeDatetime.ToLongDateString();
                        //}
                        // Shark
                        if (btc >= 500 && btc <= 1000)
                        {
                            message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🦈 Shark traded! #SELL " + btc.ToString() + "#BTC | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (btc >= 1000 && btc <= 5000)
                        {
                            message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐋 Whale traded! #SELL " + btc.ToString() + "#BTC | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (btc >= 5000)
                        {
                            message = "[Bitkub] Bitcoin #BTC " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #SELL " + btc.ToString() + "#BTC | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }

                    if (!message.Equals(""))
                    {
                        Console.WriteLine(message);
                        SendLineNotify(message);
                    }
                }

                StringBuilder sbTicker = new();
                var resultTicker = await market.GetTickerAsync(sym);
                if (resultTicker.THB_BTC != null)
                {
                    if (Convert.ToDouble(resultTicker.THB_BTC.percentChange) <= (-10))
                    {
                        sbTicker.Append("[Bitkub] Bitcoin #BTC 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.Append("Percent change is too low! 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_BTC.last)));
                        sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_BTC.high24hr)));
                        sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_BTC.low24hr)));
                        sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultTicker.THB_BTC.percentChange);
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

        static void TimerTradesBTCCallback(Object o)
        {
            Console.WriteLine("[Bitkub] Trades BTC execute time: " + DateTime.Now);
            TradesBTC().Wait();
            GC.Collect();
        }

        static async Task TradesETH()
        {
            try
            {
                IBitkubMarket market = new BitkubMarket();
                string sym = "THB_ETH";
                int limit = 30;
                var resultTrades = await market.GetTradesAsync(sym, limit);
                foreach (var value in resultTrades.result)
                {
                    string message = "";
                    JArray itemArray = JArray.Parse(value.ToString());

                    Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    DateTime tradeDatetime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    tradeDatetime = tradeDatetime.AddSeconds((double)itemArray[0]).ToLocalTime();

                    if (itemArray.Last.ToString().Equals("BUY"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double eth = (double)itemArray[2];
                        // Shark
                        if (eth >= 500 && eth <= 1000)
                        {
                            message = "[Bitkub] Ethereum #ETH " + Environment.NewLine + " 🦈 Shark traded! #BUY " + eth.ToString() + "#ETH | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (eth >= 1000 && eth <= 5000)
                        {
                            message = "[Bitkub] Ethereum #ETH " + Environment.NewLine + " 🐋 Whale traded! #BUY " + eth.ToString() + "#ETH | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (eth >= 5000)
                        {
                            message = "[Bitkub] Ethereum #ETH " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #BUY " + eth.ToString() + "#ETH | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }
                    if (itemArray.Last.ToString().Equals("SELL"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double eth = (double)itemArray[2];
                        // Shark
                        if (eth >= 500 && eth <= 1000)
                        {
                            message = "[Bitkub] Ethereum #ETH " + Environment.NewLine + " 🦈 Shark traded! #SELL " + eth.ToString() + "#ETH | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (eth >= 1000 && eth <= 5000)
                        {
                            message = "[Bitkub] Ethereum #ETH " + Environment.NewLine + " 🐋 Whale traded! #SELL " + eth.ToString() + "#ETH | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (eth >= 5000)
                        {
                            message = "[Bitkub] Ethereum #ETH " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #SELL " + eth.ToString() + "#ETH | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }

                    if (!message.Equals(""))
                    {
                        Console.WriteLine(message);
                        SendLineNotify(message);
                    }
                }

                StringBuilder sbTicker = new();
                var resultTicker = await market.GetTickerAsync(sym);
                if (resultTicker.THB_ETH != null)
                {
                    if (Convert.ToDouble(resultTicker.THB_ETH.percentChange) <= (-10))
                    {
                        sbTicker.Append("[Bitkub] Ethereum #ETH 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.Append("Percent change is too low! 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_ETH.last)));
                        sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_ETH.high24hr)));
                        sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_ETH.low24hr)));
                        sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultTicker.THB_ETH.percentChange);
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

        static void TimerTradesETHCallback(Object o)
        {
            Console.WriteLine("[Bitkub] Trades ETH execute time: " + DateTime.Now);
            TradesETH().Wait();
            GC.Collect();
        }

        static async Task TradesBNB()
        {
            try
            {
                IBitkubMarket market = new BitkubMarket();
                string sym = "THB_BNB";
                int limit = 30;
                var resultTrades = await market.GetTradesAsync(sym, limit);
                foreach (var value in resultTrades.result)
                {
                    string message = "";
                    JArray itemArray = JArray.Parse(value.ToString());

                    Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    DateTime tradeDatetime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    tradeDatetime = tradeDatetime.AddSeconds((double)itemArray[0]).ToLocalTime();

                    if (itemArray.Last.ToString().Equals("BUY"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double bnb = (double)itemArray[2];
                        // Shark
                        if (bnb >= 500 && bnb <= 1000)
                        {
                            message = "[Bitkub] Binance #BNB " + Environment.NewLine + " 🦈 Shark traded! #BUY " + bnb.ToString() + "#BNB | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (bnb >= 1000 && bnb <= 5000)
                        {
                            message = "[Bitkub] Binance #BNB " + Environment.NewLine + " 🐋 Whale traded! #BUY " + bnb.ToString() + "#BNB | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (bnb >= 5000)
                        {
                            message = "[Bitkub] Binance #BNB " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #BUY " + bnb.ToString() + "#BNB | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }
                    if (itemArray.Last.ToString().Equals("SELL"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double bnb = (double)itemArray[2];
                        // Shark
                        if (bnb >= 500 && bnb <= 1000)
                        {
                            message = "[Bitkub] Binance #BNB " + Environment.NewLine + " 🦈 Shark traded! #SELL " + bnb.ToString() + "#BNB | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (bnb >= 1000 && bnb <= 5000)
                        {
                            message = "[Bitkub] Binance #BNB " + Environment.NewLine + " 🐋 Whale traded! #SELL " + bnb.ToString() + "#BNB | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (bnb >= 5000)
                        {
                            message = "[Bitkub] Binance #BNB " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #SELL " + bnb.ToString() + "#BNB | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }

                    if (!message.Equals(""))
                    {
                        Console.WriteLine(message);
                        SendLineNotify(message);
                    }
                }

                StringBuilder sbTicker = new();
                var resultTicker = await market.GetTickerAsync(sym);
                if (resultTicker.THB_KSM != null)
                {
                    if (Convert.ToDouble(resultTicker.THB_KSM.percentChange) <= (-10))
                    {
                        sbTicker.Append("[Bitkub] Binance #BNB 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.Append("Percent change is too low! 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_BNB.last)));
                        sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_BNB.high24hr)));
                        sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_BNB.low24hr)));
                        sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultTicker.THB_KSM.percentChange);
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

        static void TimerTradesBNBCallback(Object o)
        {
            Console.WriteLine("[Bitkub] Trades BNB execute time: " + DateTime.Now);
            TradesBNB().Wait();
            GC.Collect();
        }

        static async Task TradesCOMP()
        {
            try
            {
                IBitkubMarket market = new BitkubMarket();
                string sym = "THB_COMP";
                int limit = 30;
                var resultTrades = await market.GetTradesAsync(sym, limit);
                foreach (var value in resultTrades.result)
                {
                    string message = "";
                    JArray itemArray = JArray.Parse(value.ToString());

                    Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    DateTime tradeDatetime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    tradeDatetime = tradeDatetime.AddSeconds((double)itemArray[0]).ToLocalTime();

                    if (itemArray.Last.ToString().Equals("BUY"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double comp = (double)itemArray[2];
                        // Shark
                        if (comp >= 500 && comp <= 1000)
                        {
                            message = "[Bitkub] Compound #COMP " + Environment.NewLine + " 🦈 Shark traded! #BUY " + comp.ToString() + "#COMP | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (comp >= 1000 && comp <= 5000)
                        {
                            message = "[Bitkub] Compound #COMP " + Environment.NewLine + " 🐋 Whale traded! #BUY " + comp.ToString() + "#COMP | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (comp >= 5000)
                        {
                            message = "[Bitkub] Compound #COMP " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #BUY " + comp.ToString() + "#COMP | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }
                    if (itemArray.Last.ToString().Equals("SELL"))
                    {
                        double priceTHB = (double)itemArray[1];
                        double comp = (double)itemArray[2];
                        // Shark
                        if (comp >= 500 && comp <= 1000)
                        {
                            message = "[Bitkub] Compound #COMP " + Environment.NewLine + " 🦈 Shark traded! #SELL " + comp.ToString() + "#COMP | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Whale
                        else if (comp >= 1000 && comp <= 5000)
                        {
                            message = "[Bitkub] Compound #COMP " + Environment.NewLine + " 🐋 Whale traded! #SELL " + comp.ToString() + "#COMP | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                        // Humpback Whale
                        else if (comp >= 5000)
                        {
                            message = "[Bitkub] Compound #COMP " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #SELL " + comp.ToString() + "#COMP | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
                        }
                    }

                    if (!message.Equals(""))
                    {
                        Console.WriteLine(message);
                        SendLineNotify(message);
                    }
                }

                StringBuilder sbTicker = new();
                var resultTicker = await market.GetTickerAsync(sym);
                if (resultTicker.THB_KSM != null)
                {
                    if (Convert.ToDouble(resultTicker.THB_KSM.percentChange) <= (-10))
                    {
                        sbTicker.Append("[Bitkub] Compound #COMP 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.Append("Percent change is too low! 🔥🔥🔥" + Environment.NewLine);
                        sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_COMP.last)));
                        sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_COMP.high24hr)));
                        sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_COMP.low24hr)));
                        sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultTicker.THB_COMP.percentChange);
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

        static void TimerTradesCOMPCallback(Object o)
        {
            Console.WriteLine("[Bitkub] Trades COMP execute time: " + DateTime.Now);
            TradesCOMP().Wait();
            GC.Collect();
        }

        //static async Task TradesKSM()
        //{
        //    try
        //    {
        //        IBitkubMarket market = new BitkubMarket();
        //        string sym = "THB_KSM";
        //        int limit = 30;
        //        var resultTrades = await market.GetTradesAsync(sym, limit);
        //        foreach (var value in resultTrades.result)
        //        {
        //            string message = "";
        //            JArray itemArray = JArray.Parse(value.ToString());

        //            Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        //            DateTime tradeDatetime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //            tradeDatetime = tradeDatetime.AddSeconds((double)itemArray[0]).ToLocalTime();

        //            if (itemArray.Last.ToString().Equals("BUY"))
        //            {
        //                double priceTHB = (double)itemArray[1];
        //                double ksm = (double)itemArray[2];
        //                //// Crab
        //                //if (btc >= 1 && btc <= 10)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🦀 Crab traded! #BUY " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                //// Octopus
        //                //else if (btc >= 10 && btc <= 50)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐙 Octopus traded! #BUY " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                //// Fish
        //                //else if (btc >= 50 && btc <= 100)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐟 Fish traded! #BUY " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                //// Dolphin
        //                //else if (btc >= 100 && btc <= 500)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐬 Dolphin traded! #BUY " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                // Shark
        //                if (ksm >= 500 && ksm <= 1000)
        //                {
        //                    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🦈 Shark traded! #BUY " + ksm.ToString() + "#KSM | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
        //                }
        //                // Whale
        //                else if (ksm >= 1000 && ksm <= 5000)
        //                {
        //                    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐋 Whale traded! #BUY " + ksm.ToString() + "#KSM | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
        //                }
        //                // Humpback Whale
        //                else if (ksm >= 5000)
        //                {
        //                    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #BUY " + ksm.ToString() + "#KSM | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
        //                }
        //            }
        //            if (itemArray.Last.ToString().Equals("SELL"))
        //            {
        //                double priceTHB = (double)itemArray[1];
        //                double ksm = (double)itemArray[2];
        //                //// Crab
        //                //if (btc >= 1 && btc <= 10)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🦀 Crab traded! #SELL " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                //// Octopus
        //                //else if (btc >= 10 && btc <= 50)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐙 Octopus traded! #SELL " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                //// Fish
        //                //else if (btc >= 50 && btc <= 100)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐟 Fish traded! #SELL " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                //// Dolphin
        //                //else if (btc >= 100 && btc <= 500)
        //                //{
        //                //    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐬 Dolphin traded! #SELL " + ksm.ToString() + " Date: " + tradeDatetime.ToLongDateString();
        //                //}
        //                // Shark
        //                if (ksm >= 500 && ksm <= 1000)
        //                {
        //                    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🦈 Shark traded! #SELL " + ksm.ToString() + "#KSM | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
        //                }
        //                // Whale
        //                else if (ksm >= 1000 && ksm <= 5000)
        //                {
        //                    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐋 Whale traded! #SELL " + ksm.ToString() + "#KSM | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
        //                }
        //                // Humpback Whale
        //                else if (ksm >= 5000)
        //                {
        //                    message = "[Bitkub] Kusama #KSM " + Environment.NewLine + " 🐳🐋 Humpback Whale traded! #SELL " + ksm.ToString() + "#KSM | " + priceTHB.ToString() + "#THB |" + " Date: " + tradeDatetime.ToLongDateString() + " " + tradeDatetime.ToShortTimeString();
        //                }
        //            }

        //            if (!message.Equals(""))
        //            {
        //                Console.WriteLine(message);
        //                SendLineNotify(message);
        //            }
        //        }

        //        StringBuilder sbTicker = new();
        //        var resultTicker = await market.GetTickerAsync(sym);
        //        if (resultTicker.THB_KSM != null)
        //        {
        //            if (Convert.ToDouble(resultTicker.THB_KSM.percentChange) <= (-10))
        //            {
        //                sbTicker.Append("[Bitkub] Kusama #KSM 🔥🔥🔥" + Environment.NewLine);
        //                sbTicker.Append("Percent change is too low! 🔥🔥🔥" + Environment.NewLine);
        //                sbTicker.AppendFormat("Last: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_KSM.last)));
        //                sbTicker.AppendFormat("High 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_KSM.high24hr)));
        //                sbTicker.AppendFormat("Low 24 hrs: {0}" + Environment.NewLine, string.Format("{0:#,0.####}", Convert.ToDouble(resultTicker.THB_KSM.low24hr)));
        //                sbTicker.AppendFormat("Percent change: {0}" + Environment.NewLine, resultTicker.THB_KSM.percentChange);
        //                sbTicker.Append("-------------------------------------" + Environment.NewLine);

        //                Console.WriteLine(sbTicker);
        //                SendLineNotify(sbTicker.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //static void TimerTradesKSMCallback(Object o)
        //{
        //    Console.WriteLine("[Bitkub] Trades KSM execute time: " + DateTime.Now);
        //    TradesKSM().Wait();
        //    GC.Collect();
        //}
    }
}
