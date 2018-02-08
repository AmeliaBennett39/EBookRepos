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
            definition def = new definition();
            dynamic result = def.getDef(word);

            System.Console.WriteLine(result); //def[0]  for 1 definition
        }
    }
}

