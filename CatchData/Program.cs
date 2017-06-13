using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

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
        //private static int SID;
        //private static StockData Stockid;

        static void Main(string[] args)
        {
            GetStock("2311");
        }
        static void GetStock(string Stockid)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;//換成UTF8避免亂碼
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            var htmlstr = wc.DownloadString(string.Format("http://pchome.megatime.com.tw/stock/sto3/ock1/sid{0}.html", Stockid));
            // Thread.Sleep(3000);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlstr);

            HtmlDocument hdc = new HtmlDocument();
            HtmlDocument hdd = new HtmlDocument();

            var table = doc.DocumentNode.SelectNodes("//*[@id='bttb']/table[2]");//抓table
            var list_tr = table.ToList<HtmlNode>()[0];//第1個table
            hdc.LoadHtml(list_tr.InnerHtml);//解析html
            var tr_in_tbl = hdc.DocumentNode.SelectNodes("//tr");
            var td_in_tr = tr_in_tbl.ToList<HtmlNode>();

            var fet = table.ToList<HtmlNode>().Skip(1).Take(17);
            var fe = fet.Select(tr =>
            {
                hdd.LoadHtml(tr.InnerHtml);

                var toStock =
                hdd.DocumentNode.SelectNodes("//td")
                .Select(tr2 => tr2.InnerText)
                .ToArray();

                var o = new StockData();
                o.SID = int.Parse(Stockid);
                o.Year = Int32.Parse(toStock[0]);
                o.Sdate = toStock[2] == "" ? null : (DateTime?)DateTime.Parse(toStock[2]);
                o.ExRdate = toStock[3] == "" ? null : (DateTime?)DateTime.Parse(toStock[3]);
                o.ExR = toStock[4] == "" ? null : (Decimal?)Decimal.Parse(toStock[4]);
                o.ExDdate = toStock[5] == "" ? null : (DateTime?)DateTime.Parse(toStock[5]);
                o.ExD = toStock[6] == "" ? null : (Decimal?)Decimal.Parse(toStock[6]);
                o.Cashdate = toStock[7] == "" ? null : (DateTime?)DateTime.Parse(toStock[7]);
                o.CashDividendTotal = toStock[10] == "" ? null : (Decimal?)Decimal.Parse(toStock[10]);
                o.StockDividendSurplus = toStock[11] == "" ? null : (Decimal?)Decimal.Parse(toStock[11]);
                o.StockDividendCR = toStock[12] == "" ? null : (Decimal?)Decimal.Parse(toStock[12]);
                o.AvgStockPrice = toStock[15] == "" ? null : (Decimal?)Decimal.Parse(toStock[15]);
                o.YieldRate = toStock[16] == "" ? null : (Decimal?)Decimal.Parse(toStock[16]);

                return o;
            }).ToList();

            
            //Dictionary<int, string> _dictionary = new Dictionary<int, string>();
            //int Year = 2000;
            //_dictionary.Add(Year, string.Format("http://goodinfo.tw/StockInfo/StockDividendSchedule.asp?STOCK_ID={0}"));
        }

    }
}
