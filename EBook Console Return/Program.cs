using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Define;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string sentence;
            int place;
            Console.WriteLine("Input the sentence.");
            sentence = System.Console.ReadLine();
            Console.WriteLine("Input position of the word (0 is first)");
            place = int.Parse(System.Console.ReadLine());

            // Create a new instance of the definition class
            definition def = new definition();
            // Make a dynamic object of the definition
            List<string> strResults = def.getDef(sentence, place);//def.getWebsterDef(sentence, place);

            for (int i = 0; i < strResults.Count(); i++)
            {
                Console.WriteLine(strResults[i]);
            }

        }
    }
}
