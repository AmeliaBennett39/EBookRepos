using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Define
{
    class definition
    {
        public dynamic getDef(string arg)
        { 
            System.Net.WebClient wc = new System.Net.WebClient();

            string url = "http://api.datamuse.com/words?sp=" + arg + "&qe=sp&md=d&max=1&lc=the";
            string webData = wc.DownloadString(url);

            var res = JsonConvert.DeserializeObject<dynamic>(webData);

            var def = res[0].defs;
  
            return def;
        }
    }
}
