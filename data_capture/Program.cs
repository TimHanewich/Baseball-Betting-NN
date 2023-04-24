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
                bool failed = false;

                try
                {
                    Console.Write("Retrieving scoreboard... ");
                    Scoreboard s = await Scoreboard.RetrieveAsync();
                    Console.WriteLine("Retrieved with " + s.Games.Length.ToString("#,##0") + " games!");

                    //Retrieve DraftKings
                    Console.Write("Retrieving DraftKings lines... ");
                    BettingLine[] lines = await BettingLine.RetrieveAsync();
                    Console.WriteLine("Retrieved with " + lines.Length.ToString("#,##0") + " lines!");

                    //Select a subset of the ESPN games that either have not started yet or are currently live (EXCLUDE past games)
                    List<Game> LiveOrUpcomingGames = new List<Game>();
                    foreach (Game g in s.Games)
                    {
                        if (g.Inning > 0)
                        {
                            LiveOrUpcomingGames.Add(g);
                        }
                        else
                        {
                            TimeSpan ts = g.StartDateUtc - DateTime.UtcNow;
                            if (ts.TotalSeconds > 0)
                            {
                                LiveOrUpcomingGames.Add(g);
                            }
                        }
                    }

                    //Collect new states
                    int new_states_added = 0;
                    int matches_found = 0;
                    foreach (Game g in LiveOrUpcomingGames)
                    {
                        StatePredictionPair spp = new StatePredictionPair();
                        spp.State = g.ToState();

                        //Find the prediction in the draftkings
                        BettingLine? matching_line = null;
                        foreach (BettingLine line in lines)
                        {

                            //Is this a potential team match?
                            bool team_match = false;
                            if (g.AwayTeamAbbreviation.ToLower().Contains(line.AwayTeamAbbreviation.ToLower()))
                            {
                                team_match = true;
                            }
                            else if (g.HomeTeamAbbreviation.ToLower().Contains(line.HomeTeamAbbreviation.ToLower()))
                            {
                                team_match = true;
                            }
                            else if (line.AwayTeamAbbreviation.ToLower().Contains(g.AwayTeamAbbreviation.ToLower()))
                            {
                                team_match = true;
                            }
                            else if (line.HomeTeamAbbreviation.ToLower().Contains(g.HomeTeamAbbreviation.ToLower()))
                            {
                                team_match = true;
                            }

                            if (team_match)
                            {
                                TimeSpan ts = g.StartDateUtc - line.StartDateUtc;
                                if (Math.Abs(ts.TotalMinutes) < 20)
                                {
                                    matching_line = line;
                                }
                            } 
                        }

                        //If we have a matching line, see if we should add it
                        if (matching_line != null)
                        {
                            matches_found = matches_found + 1;
                            spp.Prediction = matching_line.ToState();
                            bool added = db.AddIfNotStored(spp);
                            if (added)
                            {
                                new_states_added = new_states_added + 1;
                            }
                        }
                    }

                    //Print matching rate
                    Console.WriteLine(matches_found.ToString() + " / " + LiveOrUpcomingGames.Count.ToString() + " live or upcoming games matched.");

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
                catch
                {
                    failed = true;
                }
                
                if (failed)
                {
                    await Task.Delay(15000);
                }
            }
            
            
        }

        
    }
}