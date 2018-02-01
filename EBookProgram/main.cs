using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    class Result
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string word;
            word = System.Console.ReadLine();
           
            string webData = wc.DownloadString("http://api.datamuse.com/words?sp="+word+"&qe=sp&md=d");
            System.Console.WriteLine(webData);

            //JsonConvert

        }
    }
}
