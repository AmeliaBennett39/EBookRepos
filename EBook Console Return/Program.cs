using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Define;
using System.Speech;
using System.Speech.Synthesis;

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

            //SpeechSynthesizer reader = new SpeechSynthesizer();
            //reader.SpeakAsync(result);

            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {

                // Configure the audio output. 
                synth.SetOutputToDefaultAudioDevice();

                // Speak a string synchronously.
                synth.Speak((string)result[0]);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

