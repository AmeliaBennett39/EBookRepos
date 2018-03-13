using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Define
{
    class definition
    {

        public List<string> getWebsterDef(string arg, int place)
        {
            //List of definitions containing defs for all PoS
            List<string> defs = new List<string>();
            // Create new instance of webclient 
            System.Net.WebClient wc = new System.Net.WebClient();

            string word;
            word = arg.Split(' ')[place];

            Console.WriteLine(word);
            // Use the API to get definitions of the word passed
            string url = "https://www.dictionaryapi.com/api/v1/references/sd4/xml/" + word + "?key=6c1bfb6c-266f-41c8-a52a-3ac78bc616d8";
            // Return a XML using the webclient instance
            string webData = wc.DownloadString(url);

            string taggedtext;
            // Request part of speech data
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["text"] = arg;

                var response = wb.UploadValues("http://text-processing.com/api/tag/", "POST", data);
                taggedtext = Encoding.UTF8.GetString(response);

                //https://cs.nyu.edu/grishman/jet/guide/PennPOS.html
                //describes each Part of Speech label
            }

            //Part of speech using complex Penn labeling
            string complexpos;
            //Console.WriteLine(taggedtext);
            try
            {
                complexpos = taggedtext.Split(' ')[place + 2].Split('/')[1].Split(')')[0];
            }
            catch (Exception e)
            {
                complexpos = "GO";
            }
            string simplepos = SimplePartOfSpeechWebster(complexpos);

            //parse XML
            XElement definitions = XElement.Parse(webData);
            IEnumerable<XNode> a =  definitions.Descendants("entry");
            bool changeWord = false;

            for (int i = 0; i < a.Count(); i++)
            {
                XElement mainXElement = (XElement)a.ElementAt(i);
                IEnumerable<XNode> nodeContainer;
                
                string defWord = (mainXElement.Attribute((XName)"id").Value);
                defWord = defWord.Split('[')[0].Split('-')[0].Split('\'')[0];

                if (changeWord)
                {
                    word = defWord;
                    changeWord = false;
                }
                if (!defWord.ToUpper().Equals(word.ToUpper())) {
                    if (i == 0)
                        word = defWord;
                     else
                        continue;
                }
                //check for pos and word here;


                nodeContainer = mainXElement.Descendants((XName)"fl");
                if (nodeContainer.Count() == 0)
                {
                    Console.WriteLine("aa");
                    if (i == 0)
                    {
                        Console.WriteLine("changed word");
                        changeWord = true;
                    }
                    continue;
                }
                string defpos = ((XElement)nodeContainer.First()).Value;
               // Console.WriteLine(defpos);
                if (!simplepos.ToUpper().Equals(defpos.ToUpper()) && !simplepos.ToUpper().Equals("GO"))
                    continue;

                // Part of speech converted to simple labeling used in dictionaries
                
                defs.Add(((XElement)nodeContainer.First()).Value);

                nodeContainer = mainXElement.Descendants((XName)"def"); //get all the definitions under this version of the word
                if (nodeContainer.Count() == 0)
                {
                    Console.WriteLine("aa");
                    if (i == 0)
                    {
                        Console.WriteLine("changed word");
                        changeWord = true;
                    }
                    continue;
                }
                    
                XElement Xdefinitions = (XElement)nodeContainer.First();
                nodeContainer = Xdefinitions.Descendants((XName)"dt");

                int definitionCount = 0; //definition count for this particular word or part of speech
                int maxDefinitionsPerSpecificWord = 5; //most definitions allowed to be given for a specific part of speech
                for (int j = 0; j < nodeContainer.Count() && definitionCount < maxDefinitionsPerSpecificWord; j++)
                {
                    XElement Xdefinition = (XElement)nodeContainer.ElementAt(j);
                    IEnumerable<XNode> nodeContainer2 = Xdefinition.Descendants((XName)"vi");
                    for (int k = nodeContainer2.Count(); k > 0; k--)
                    {
                        nodeContainer2.ElementAt(k-1).Remove();
                    }
                    nodeContainer2 = Xdefinition.Descendants((XName)"sx");
                    for (int k = nodeContainer2.Count(); k > 0; k--)
                    {
                        nodeContainer2.ElementAt(k-1).Remove();
                    }
                    nodeContainer2 = Xdefinition.Descendants((XName)"dx");
                    for (int k = nodeContainer2.Count(); k > 0; k--)
                    {
                        nodeContainer2.ElementAt(k-1).Remove();
                    }

                    defs.Add(Xdefinition.Value);
                    definitionCount++;

                }
            }
            return defs;



        }
        public List<string> getDef(string arg, int place)
        {
            //List of definitions containing defs for all PoS
            List<string> stringdefs = new List<string>();
            //List of definitions containing defs for only the identified PoS
            List<string> newdefs = new List<string>();

            // Create new instance of webclient 
            System.Net.WebClient wc = new System.Net.WebClient();

            string word;

            word = arg.Split(' ')[place];
            Console.WriteLine(word);
            // Use the API to get definitions of the word passed
            string url = "http://api.datamuse.com/words?sp=" + word + "&qe=sp&md=d&max=1";
            // Return a JSON using the webclient instance
            string webData = wc.DownloadString(url);

            // Parse the JSON
            var res = JsonConvert.DeserializeObject<dynamic>(webData);

            // Text to be returned from text-processing.com showing parts of speech for the sentence
            string taggedtext;
            // Request part of speech data
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["text"] = arg;

                var response = wb.UploadValues("http://text-processing.com/api/tag/", "POST", data);
                taggedtext = Encoding.UTF8.GetString(response);

                //https://cs.nyu.edu/grishman/jet/guide/PennPOS.html
                //describes each Part of Speech label
            }

            //Part of speech using complex Penn labeling
            string complexpos;
            try
            {
                complexpos = taggedtext.Split(' ')[place + 2].Split('/')[1].Split(')')[0];
            } catch (Exception e)
            {
                complexpos = "GO";
            }

            // Part of speech converted to simple labeling used in dictionaries
            string simplepos = SimplePartOfSpeech(complexpos);

           

            // Return an object of the definitions
            JArray def = res[0].defs;

            

            if (simplepos.Equals("XX") || def == null || def.Count() == 0)
            {
                if (complexpos.Equals("FW"))
                    stringdefs.Add("The word is foreign");
                else
                    stringdefs.Add("No definitions could be found");
                Console.WriteLine(complexpos);
                return stringdefs;
            }

            //Load JSON text into string list
            for (int i = 0; i < def.Count(); i++)
            {
                stringdefs.Add(def[i].ToString());
            }

            //For some identified PoS we will display all definitions
            if (simplepos.Equals("GO")){ //ignore part of speech
                return stringdefs;
            }

            //Load any definitions that share the PoS with the word in context
            //into a new list to be displayed
            for (int i = 0; i < stringdefs.Count(); i++)
            {

                string curdef = stringdefs[i];
                string curpos = curdef.Split('\t')[0];
                if (curpos.Equals(simplepos))
                {
                    newdefs.Add(curdef);
                }

            }

            //If no definitions are found for this PoS, display all definitions
            if (newdefs.Count>0)
                return newdefs;
            return stringdefs;
           //return def;
        }

        public string SimplePartOfSpeechWebster(string pos) {
            string simplepos;
            switch (pos)
            {
                case "CD":
                case "NN":
                case "NNP":
                case "NNPS":
                case "PDT":                
                    simplepos = "noun";
                    break;
                case "MD":
                case "VB":
                case "VBG":
                case "VBD":
                case "VBP":
                case "VBZ":
                    simplepos = "verb";
                    break;
                case "JJ":
                case "JJR":
                case "JJS":
                case "WDT":
                    simplepos = "adjective";
                    break;
                case "RB":
                case "RBR":
                case "RBS":
                case "TO":
                case "WRB":
                    simplepos = "adverb";
                    break;
                case "CC":
                    simplepos = "conjunction";
                    break;           
                case "DT":
                    simplepos = "definite article";
                    break;
                case "UH":
                    simplepos = "interjection";
                    break;
                case "WP":
                case "WP$":
                case "PRP$":
                case "PRP":
                    simplepos = "pronoun";
                    break;
                case "FW":
                case "SYM":              
                    simplepos = "XX";
                    break;
                case "RP":
                default:
                    simplepos = "GO";
                    break;
            }

            return simplepos;
        }
        public string SimplePartOfSpeech(string pos)
        {
            string simplepos;
            switch (pos)
            {
                case "CD":
                case "NN":
                case "NNP":
                case "NNPS":
                case "PRP":
                case "PDT":
                case "PRP$":
                    simplepos = "n";
                    break;
                case "MD":
                case "VB":
                case "VBG":
                case "VBD":
                case "VBP":
                case "VBZ":

                    simplepos = "v";
                    break;
                case "JJ":
                case "JJR":
                case "JJS":
                    simplepos = "adj";
                    break;
                case "RB":
                case "RBR":
                case "RBS":
                    simplepos = "adv";
                    break;
                case "CC":
                case "FW":
                case "DT":
                case "TO":
                case "SYM":
                case "UH":
                case "WP":
                case "WDT":
                case "WP$":
                    simplepos = "XX";
                    break;
                case "RP":
                case "WRB":
                default:
                    simplepos = "GO";
                    break;

                

            }
            return simplepos;
        }
    }
}
