using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ESPN
{
    public class Game
    {
        //Not included in state
        public int Id {get; set;}
        public float? WinProbability {get; set;} //1.0 would be a 100% probability for the away team. -1.0 is a 100% probability for the home team. If this is null, it means it was not available in  the initial read from the Scoreboard (one page) and a call to the game page is required.
        public string AwayTeamAbbreviation {get; set;}
        public string HomeTeamAbbreviation {get; set;}
        public DateTime StartDateUtc {get; set;}


        public float AwayTeamWinningRecord {get; set;}
        public float HomeTeamWinningRecord {get; set;}
        public int AwayTeamRuns {get; set;}
        public int HomeTeamRuns {get; set;}
        public int AwayTeamHits {get; set;}
        public int HomeTeamHits {get; set;}
        public int AwayTeamErrors {get; set;}
        public int HomeTeamErrors {get; set;}
        public int Inning {get; set;} //If the inning is 0, it means the game either hasn't started yet or is over
        public Team BattingTeam {get; set;}
        public int Outs {get; set;}
        public int Balls {get; set;}
        public int Strikes {get; set;}
        public bool ManOnFirst {get; set;}
        public bool ManOnSecond {get; set;}
        public bool ManOnThird {get; set;}

        public Game()
        {
            AwayTeamAbbreviation = string.Empty;
            HomeTeamAbbreviation = string.Empty;
        }

        public float[] ToState()
        {
            List<float> ToReturn = new List<float>();

            ToReturn.Add(AwayTeamWinningRecord);
            ToReturn.Add(HomeTeamWinningRecord);
            ToReturn.Add(AwayTeamRuns);
            ToReturn.Add(HomeTeamRuns);
            ToReturn.Add(AwayTeamHits);
            ToReturn.Add(HomeTeamHits);
            ToReturn.Add(AwayTeamErrors);
            ToReturn.Add(HomeTeamErrors);
            ToReturn.Add(Inning);
            ToReturn.Add(Convert.ToSingle(BattingTeam));
            ToReturn.Add(Outs);
            ToReturn.Add(Balls);
            ToReturn.Add(Strikes);
            ToReturn.Add(Convert.ToInt32(ManOnFirst));
            ToReturn.Add(Convert.ToInt32(ManOnSecond));
            ToReturn.Add(Convert.ToInt32(ManOnThird));

            return ToReturn.ToArray();
        }

        public static bool EquivalentStates(float[] s1, float[] s2)
        {
            if (s1.Length != s2.Length)
            {
                return false;
            }
            else
            {
                for (int t = 0; t < s1.Length; t++)
                {
                    if (s1[t] != s2[t])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    
        public async Task GetWinProbabilityAsync()
        {
            HttpClient hc = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri("https://www.espn.com/mlb/game/_/gameId/" + Id.ToString());
            HttpResponseMessage response = await hc.SendAsync(req);
            string content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unable to get win probability for game '" + Id.ToString() + "'. Request to ESPN returned " + response.StatusCode.ToString() + "!");
            }

            //Get JSON
            int loc1 = content.IndexOf("window['__espnfitt__']=");
            loc1 = content.IndexOf("=", loc1 + 1);
            int loc2 = content.IndexOf(";</script>");
            string json_text = content.Substring(loc1 + 1, loc2 - loc1 - 1);
            JObject json = JObject.Parse(json_text);

            //Get the home + away team abbreviation
            string home_team = "";
            string away_team = "";
            JToken? hta = json.SelectToken("page.content.gamepackage.gmStrp.tms[0].abbrev");
            if (hta != null)
            {
                home_team = hta.ToString();
            }
            JToken? ata = json.SelectToken("page.content.gamepackage.gmStrp.tms[1].abbrev");
            if (ata != null)
            {
                away_team = ata.ToString();
            }

            //Get the line
            JToken? dets = json.SelectToken("page.content.gamepackage.gmStrp.odds.dets");
            if (dets != null)
            {
                string line = dets.ToString();

                //Multiplier - away or home?
                float multiplier = 1f;
                if (line.ToLower().Contains(home_team.ToLower()))
                {
                    multiplier = -1f;
                }
                
                string odds = line;
                odds = odds.ToLower();
                odds = odds.Replace(home_team.ToLower(), "");
                odds = odds.Replace(away_team.ToLower(), "");
                odds = odds.Trim();

                if (odds == "even") //If the money line is even (i.e. both are -110), it will say "even"
                {
                    WinProbability = 0.0f;
                }
                else //i.e. "-125"
                {
                    float win_probability = Toolkit.MoneyLineToImpliedProbability(Convert.ToInt32(odds));
                    win_probability = win_probability * multiplier;
                    WinProbability = win_probability;
                }
            }


        }
    }
}