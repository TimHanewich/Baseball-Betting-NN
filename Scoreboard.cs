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

        public static async Task RetrieveAsync()
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

            //Parse each
            List<Game> games = new List<Game>();
            for (int t = 1; t < games_html.Length; t++)
            {
                Game g = new Game();
                string game_html = games_html[t];

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

                //Get the inning


            }

            
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