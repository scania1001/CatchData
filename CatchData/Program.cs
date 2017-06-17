using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace CatchData
{
    public class StockData
    {
        public int SID { get; set; }
        public int Year { get; set; }
        public DateTime? Sdate { get; set; }
        public DateTime? ExRdate { get; set; }
        public Decimal? ExR { get; set; }
        public DateTime? ExDdate { get; set; }
        public Decimal? ExD { get; set; }
        public DateTime? Cashdate { get; set; }
        public Decimal? CashDividendTotal { get; set; }
        public Decimal? StockDividendSurplus { get; set; }
        public Decimal? StockDividendCR { get; set; }
        public Decimal? AvgStockPrice { get; set; }
        public Decimal? YieldRate { get; set; }
    }


    public class Program
    {
        //static Dictionary<string, StockData> _dictionary = new Dictionary<string, StockData>();

        static void Main(string[] args)
        {


        }
        public static Dictionary<string, StockData> GetStock(string Stockid)
        {
            Dictionary<string, StockData> _dictionary = new Dictionary<string, StockData>();

            WebClient wc = new WebClient();
            //wc.Encoding = Encoding.UTF8;//換成UTF8避免亂碼
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            var nvc = new NameValueCollection();
            nvc["is_check"] = "1";
            var buffer = wc.UploadValues(string.Format("http://pchome.megatime.com.tw/stock/sto3/ock1/sid{0}.html", Stockid), nvc);
            // Thread.Sleep(3000);
            var htmlstr = Encoding.UTF8.GetString(buffer);
            MemoryStream ms = new MemoryStream(buffer);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlstr);

            HtmlDocument hdc = new HtmlDocument();

            //List<StockData> Stock = new List<StockData>();

            var table = doc.DocumentNode.SelectNodes("//*[@id='bttb']/table[2]");//抓table//*[@id="bttb"]/table[2]
            var list_tr = table.ToList<HtmlNode>()[0];//第1個table
            hdc.LoadHtml(list_tr.InnerHtml);//解析html
            var tr_in_tbl = hdc.DocumentNode.SelectNodes("//tr");
            var td_in_tr = tr_in_tbl.ToList<HtmlNode>();

            var fet = td_in_tr.Skip(3).Take(16);
            var fe = fet.Select(tr =>
            {
                HtmlDocument hdctmp = new HtmlDocument();
                hdctmp.LoadHtml(tr.InnerHtml);
                var o = new StockData();
                var toStock =
                hdctmp.DocumentNode.SelectNodes("//td")
                .Select(tr2 => tr2.InnerText)
                .ToArray();


                o.SID                  = Int32.Parse(Stockid);
                o.Year                 = Int32.Parse(toStock[0]) - 1;
                o.ExRdate              = 
                    toStock[1].ToString() == ""  ? null :
                    toStock[1].ToString() == "-" ? 
                        (DateTime?)DateTime.Parse(toStock[0] + "-01-01") : 
                        (DateTime?)DateTime.Parse(toStock[0] + "-" + toStock[1]);
                o.CashDividendTotal    = toStock[2]                == "" ? null : (Decimal?) Decimal. Parse(toStock[2]);
                o.ExDdate              = 
                    toStock[3].ToString() == "" ? null :
                    toStock[3].ToString() == "-" ?
                        (DateTime?)DateTime.Parse(toStock[0] + "-01-01") : 
                        (DateTime?)DateTime.Parse(toStock[0] + "-" + toStock[3]);
                o.StockDividendSurplus = toStock[4].ToString()     == "" ? null : (Decimal?) Decimal. Parse(toStock[4]);
                o.StockDividendCR      = toStock[5].ToString()     == "" ? null : (Decimal?) Decimal. Parse(toStock[5]);
                _dictionary.Add(toStock[0], o);
                return o;
            }).ToList();

            return _dictionary;


        }

    }
}
