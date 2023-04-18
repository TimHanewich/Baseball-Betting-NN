using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace ESPN
{
    public class Program
    {
        public static void Main(string[] args)
        {

            float[] f1 = new float[]{0.4f, 0.5f, 0.3f};
            float[] f2 = new float[]{0.4f, 0.5f, 0.3f};
            Console.WriteLine(Game.EquivalentStates(f1, f2));
            Console.ReadLine();

            
            Scoreboard s = Scoreboard.RetrieveAsync().Result;
            foreach (Game g in s.Games)
            {
                Console.WriteLine(JsonConvert.SerializeObject(g, Formatting.Indented));
                Console.ReadLine();
                Console.Clear();
            }

            

        }
    }
}