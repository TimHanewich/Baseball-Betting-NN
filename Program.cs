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

            
            Scoreboard s = Scoreboard.RetrieveAsync().Result;
            foreach (Game g in s.Games)
            {
                StateProbabilityPair sbp = g.ToStateProbabilityPairAsync().Result;
                Console.WriteLine(JsonConvert.SerializeObject(sbp, Formatting.Indented));
            }
            

        }

        
    }
}