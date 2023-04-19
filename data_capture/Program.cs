using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using DraftKings;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ESPN
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            RunAsync().Wait();
        }

        public static async Task RunAsync()
        {
            VirtualDB db = new VirtualDB(@"C:\Users\timh\Downloads\tah\nn\db.jsonl");

            while (true)
            {
                Console.Write("Retrieving scoreboard... ");
                Scoreboard s = await Scoreboard.RetrieveAsync();
                Console.WriteLine("Retrieved!");

                //Retrieve DraftKings
                Console.Write("Retrieving DraftKings lines... ");
                BettingLine[] lines = await BettingLine.RetrieveAsync();
                Console.WriteLine("Retrieved!");

                //Collect new states
                int new_states_added = 0;
                foreach (Game g in s.Games)
                {
                    StatePredictionPair spp = new StatePredictionPair();
                    spp.State = g.ToState();

                    //Find the prediction in the draftkinds
                    foreach (BettingLine line in lines)
                    {
                        if (line.AwayTeamAbbreviation == g.AwayTeamAbbreviation && line.HomeTeamAbbreviation == g.HomeTeamAbbreviation)
                        {
                            spp.Prediction = line.ToState();
                            bool added = db.AddIfNotStored(spp);
                            if (added)
                            {
                                new_states_added = new_states_added + 1;
                            }
                        }
                    }
                }

                //Print progress
                if (new_states_added > 0)
                {
                    Console.WriteLine(new_states_added.ToString("#,##0") + " new frames added!");
                }
                

                //Wait
                TimeSpan ToWait = new TimeSpan(0, 5, 0); //Default, if no games are being played right now, is 5 minutes
                
                //But, if a single game is being played right now, wait only 5 seconds
                foreach (Game g in s.Games)
                {
                    if (g.Inning != 0)
                    {
                        ToWait = new TimeSpan(0, 0, 5);
                    }
                }

                //Wait
                Console.Write("Waiting " + ToWait.TotalSeconds.ToString("#,##0") + " seconds before cycling... ");
                await Task.Delay(ToWait);
                Console.WriteLine("Moving on!");
            }
            
            
        }

        
    }
}