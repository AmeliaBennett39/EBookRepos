using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.WebClient wc = new System.Net.WebClient();

            string word;
            word = System.Console.ReadLine();

            string url = "http://api.datamuse.com/words?sp=" + word + "&qe=sp&md=d&max=1&lc=the";
            string webData = wc.DownloadString(url);

            var res = JsonConvert.DeserializeObject<dynamic>(webData);

            var def = res[0].defs;
            System.Console.WriteLine(def); //def[0]  for 1 definition


        }
    }
}

