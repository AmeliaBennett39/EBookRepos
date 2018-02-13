using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Define;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string word;
            word = System.Console.ReadLine();
            // Create a new instance of the definition class
            definition def = new definition();
            // Make a dynamic object of the definition
            dynamic result = def.getDef(word);

            System.Console.WriteLine(result); //def[0]  for 1 definition
        }
    }
}

