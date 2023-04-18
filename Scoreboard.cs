using System;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ESPN
{
    public class Scoreboard
    {
        public Game[] Games {get; set;}
        public DateTime RetrievedAtUtc {get; set;}

        public Scoreboard()
        {
            Games = new Game[]{};
        }

        public static async Task<Scoreboard> RetrieveAsync()
        {
            HttpClient hc = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri("https://www.espn.com/mlb/scoreboard");
            HttpResponseMessage resp = await hc.SendAsync(req);
            string content = await resp.Content.ReadAsStringAsync();

            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Request to ESPN scoreboard returned '" + resp.StatusCode.ToString() + "'!");
            }

            //Get JSON data
            int loc1 = content.IndexOf("window['__espnfitt__']");
            loc1 = content.IndexOf("=", loc1 + 1);
            int loc2 = content.IndexOf(";</script>", loc1 + 1);
            string json = content.Substring(loc1 + 1, loc2 - loc1 - 1);
            JObject root = JObject.Parse(json);

            //Get the events from JSON
            JArray games_json = new JArray();
            JToken? evts = root.SelectToken("page.content.scoreboard.evts");
            if (evts != null)
            {
                games_json = (JArray)evts;
            }

            //Parse
            loc1 = content.IndexOf("Card__Header Card__Header--presby");
            loc1 = content.IndexOf("<div>", loc1 + 1);
            loc2 = content.IndexOf("fitt-adbox-native-betting", loc1 + 1);
            string games_content = content.Substring(loc1 + 1, loc2 - loc1 - 1);
            string[] games_html = games_content.Split(new string[]{"Scoreboard bg-clr-white flex flex-auto justify-between"}, StringSplitOptions.RemoveEmptyEntries);
            

            //Parse each
            List<Game> games = new List<Game>();
            for (int t = 1; t < games_html.Length; t++)
            {
                Game g = new Game();
                string game_html = games_html[t];

                //Get the id
                loc1 = game_html.IndexOf("id=");
                loc1 = game_html.IndexOf("\"", loc1 + 1);
                loc2 = game_html.IndexOf("\"", loc1 + 1);
                g.Id = Convert.ToInt32(game_html.Substring(loc1 + 1, loc2 - loc1 - 1));

                //Get the away team record
                loc1 = game_html.IndexOf("class=\"ScoreboardScoreCell__Record\">");
                loc1 = game_html.IndexOf(">", loc1 + 1);
                loc2 = game_html.IndexOf("<", loc1 + 1);
                string away_record = game_html.Substring(loc1 + 1, loc2 - loc1 - 1);
                g.AwayTeamWinningRecord = RecordToPercentage(away_record);
                
                //Get the home team record
                loc1 = game_html.IndexOf("ScoreboardScoreCell__Item--home");
                loc1 = game_html.IndexOf("class=\"ScoreboardScoreCell__Record\">", loc1 + 1);
                loc1 = game_html.IndexOf(">", loc1 + 1);
                loc2 = game_html.IndexOf("<", loc1 + 1);
                string home_record = game_html.Substring(loc1 + 1, loc2 - loc1 - 1);
                g.HomeTeamWinningRecord = RecordToPercentage(home_record);

                //Get the inning, runs, etc.
                foreach (JObject jo in games_json)
                {
                    JProperty? prop_id = jo.Property("id");
                    if (prop_id != null)
                    {
                        int id = Convert.ToInt32(prop_id.Value.ToString());
                        if (id == g.Id)
                        {

                            //Get the inning & batting team
                            JToken? sdetail = jo.SelectToken("status.detail");
                            if (sdetail != null)
                            {
                                string detail = sdetail.ToString();

                                //Batting team
                                if (detail.ToLower().Contains("bot"))
                                {
                                    g.BattingTeam = Team.Home;
                                }
                                else if (detail.ToLower().Contains("top"))
                                {
                                    g.BattingTeam = Team.Away;
                                }
                                else if (detail.ToLower().Contains("end"))
                                {
                                    g.BattingTeam = Team.Away;
                                }
                                else if (detail.ToLower().Contains("mid"))
                                {
                                    g.BattingTeam = Team.Away;
                                }

                                //Inning number
                                if (detail.Contains("1st"))
                                {
                                    g.Inning = 1;
                                }
                                else if (detail.Contains("2nd"))
                                {
                                    g.Inning = 2;
                                }
                                else if (detail.Contains("3rd"))
                                {
                                    g.Inning = 3;
                                }
                                else if (detail.Contains("4th"))
                                {
                                    g.Inning = 4;
                                }
                                else if (detail.Contains("5th"))
                                {
                                    g.Inning = 5;
                                }
                                else if (detail.Contains("6th"))
                                {
                                    g.Inning = 6;
                                }
                                else if (detail.Contains("7th"))
                                {
                                    g.Inning = 7;
                                }
                                else if (detail.Contains("8th"))
                                {
                                    g.Inning = 8;
                                }
                                else if (detail.Contains("9th"))
                                {
                                    g.Inning = 9;
                                }
                                else if (detail.Contains("10th"))
                                {
                                    g.Inning = 10;
                                }
                                else if (detail.Contains("12th"))
                                {
                                    g.Inning = 11;
                                }
                                else if (detail.Contains("12th"))
                                {
                                    g.Inning = 12;
                                }
                                else if (detail.Contains("13th"))
                                {
                                    g.Inning = 13;
                                }
                                else if (detail.Contains("14th"))
                                {
                                    g.Inning = 14;
                                }
                                else if (detail.Contains("15th"))
                                {
                                    g.Inning = 15;
                                }
                                else if (detail.Contains("16th"))
                                {
                                    g.Inning = 16;
                                }
                                else if (detail.Contains("17th"))
                                {
                                    g.Inning = 17;
                                }
                                else if (detail.Contains("18th"))
                                {
                                    g.Inning = 18;
                                }
                                else
                                {
                                    g.Inning = 0; //inning of 0 means the game isn't active yet, or is finished already
                                }
                            }
                        
                            //home team runs
                            JToken? htr = jo.SelectToken("competitors[0].runs");
                            if (htr != null)
                            {
                                g.HomeTeamRuns = Convert.ToInt32(htr.ToString());
                            }

                            //away team runs
                            JToken? atr = jo.SelectToken("competitors[1].runs");
                            if (htr != null)
                            {
                                g.AwayTeamRuns = Convert.ToInt32(atr.ToString());
                            }
                        
                        }
                    }
                }

                //Get the number of balls, strikes, and outs
                loc1 = game_html.IndexOf("BaseballSituation__PitchesOuts");
                loc2 = game_html.IndexOf("<p", loc1 + 1);
                if (loc1 > -1 && loc2 > -1)
                {
                    string balls_strikes_outs_section = game_html.Substring(loc1 + 1, loc2 - loc1 - 1);
                    string[] bso_parts = balls_strikes_outs_section.Split(new string[]{"class=\"pitches"}, StringSplitOptions.RemoveEmptyEntries);
                    
                    //Balls
                    g.Balls = bso_parts[1].Split(new string[]{"active"}, StringSplitOptions.RemoveEmptyEntries).Length - 1;
                    g.Strikes = bso_parts[2].Split(new string[]{"active"}, StringSplitOptions.RemoveEmptyEntries).Length - 1;
                    g.Outs = bso_parts[3].Split(new string[]{"active"}, StringSplitOptions.RemoveEmptyEntries).Length - 1;

                    Console.WriteLine(g.Balls.ToString() + " " + g.Strikes.ToString() + " " + g.Outs.ToString());
                    
                }

                //Man on first
                g.ManOnFirst = game_html.Contains("first-base is--active");
                g.ManOnSecond = game_html.Contains("second-base is--active");
                g.ManOnThird = game_html.Contains("third-base is--active");
                


                //Add
                games.Add(g);
            }

            Scoreboard ToReturn = new Scoreboard();
            ToReturn.RetrievedAtUtc = DateTime.UtcNow;
            ToReturn.Games = games.ToArray();
            return ToReturn;
        }

        private static float RecordToPercentage(string record)
        {
            string[] parts = record.Split(new string[]{"-"}, StringSplitOptions.RemoveEmptyEntries);
            float wins = Convert.ToSingle(parts[0]);
            float losses = Convert.ToSingle(parts[1]);
            return wins / (wins + losses);
        }
    }
}