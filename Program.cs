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

            VirtualDB db = new VirtualDB(@"C:\Users\timh\Downloads\tah\nn\db.jsonl");
            
            Scoreboard s = Scoreboard.RetrieveAsync().Result;
            foreach (Game g in s.Games)
            {
                StateProbabilityPair? spp = null;
                try
                {
                    spp = g.ToStateProbabilityPairAsync().Result;
                }
                catch
                {
                    Console.WriteLine(g.Id.ToString() + " did not work.");
                }
                
                if (spp != null)
                {
                    Console.Write("Writing " + g.Id.ToString() + "'... ");
                    db.AddIfNotStored(spp);
                    Console.WriteLine("Done!");
                }
                
            }
            

        }

        
    }
}