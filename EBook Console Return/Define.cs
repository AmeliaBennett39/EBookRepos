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
            // Create new instance of webclient 
            System.Net.WebClient wc = new System.Net.WebClient();

            // Use the API to get definitions of the word passed
            string url = "http://api.datamuse.com/words?sp=" + arg + "&qe=sp&md=d&max=1&lc=the";
            // Return a JSON using the webclient instance
            string webData = wc.DownloadString(url);

            // Parse the JSON
            var res = JsonConvert.DeserializeObject<dynamic>(webData);

            // Return an object of the definitions
            var def = res[0].defs;
            return def;
        }
    }
}
