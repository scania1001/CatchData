using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatchData;
using HtmlAgilityPack;

namespace CatchData.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var mm = CatchData.Program.GetStock("2311");
            Console.ReadLine();
        }
    }
}
