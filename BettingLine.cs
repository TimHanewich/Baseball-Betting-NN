using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DraftKings
{
    public class BettingLine
    {
        public int Id {get; set;}
        public string AwayTeam {get; set;}
        public string AwayTeamAbbreviation {get; set;}
        public string HomeTeam {get; set;}
        public string HomeTeamAbbreviation {get; set;}

        //Used in the model
        public float RunLine {get; set;} //i.e. 1.5 or -1.5 where you would choose either
        public float TotalLine {get; set;} //Total # of combined runs in the game
        public float AwayTeamMoneyLine {get; set;} //i.e. -220
        public float HomeTeamMoneyLine {get; set;} //i.e. +180

        public BettingLine()
        {
            AwayTeam = string.Empty;
            AwayTeamAbbreviation = string.Empty;
            HomeTeam = String.Empty;
            HomeTeamAbbreviation = string.Empty;
        }

        public static async Task<BettingLine[]> RetrieveAsync()
        {
            HttpClient hc = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri("https://sportsbook.draftkings.com/leagues/baseball/mlb");
            HttpResponseMessage response = await hc.SendAsync(req);
            string content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unable to get betting lines from DraftKings. DraftKings returned status code " + response.StatusCode.ToString());
            }

            //Get JSON
            int loc1 = content.IndexOf("window.__INITIAL_STATE__");
            loc1 = content.IndexOf("=", loc1 +1);
            int loc2 = content.IndexOf("window.__serverDate", loc1 + 1);
            loc2 = content.LastIndexOf(";", loc2);
            string json_text = content.Substring(loc1 + 1, loc2 - loc1 - 1).Trim();
            JObject root = JObject.Parse(json_text);

            List<BettingLine> ToReturn = new List<BettingLine>();

            //Get the events
            JToken? events = root.SelectToken("eventGroups.84240.events"); //A jobject
            if (events != null)
            {

                JObject events_jo = (JObject)events;
                foreach (JProperty prop in events_jo.Properties())
                {
                    JObject e = (JObject)prop.Value;

                    BettingLine ThisBettingLine = new BettingLine();

                    //Id
                    JProperty? prop_eventId = e.Property("eventId");
                    if (prop_eventId != null)
                    {
                        ThisBettingLine.Id = Convert.ToInt32(prop_eventId.Value.ToString());
                    }

                    //Away team (team 1)
                    JProperty? prop_teamName1 = e.Property("teamName1");
                    if (prop_teamName1 != null)
                    {
                        ThisBettingLine.AwayTeam = prop_teamName1.Value.ToString();
                    }

                    //home team (team 2)
                    JProperty? prop_teamName2 = e.Property("teamName2");
                    if (prop_teamName2 != null)
                    {
                        ThisBettingLine.HomeTeam = prop_teamName2.Value.ToString();
                    }

                    //Team 1 short name
                    JProperty? prop_teamShortName1 = e.Property("teamShortName1");
                    if (prop_teamShortName1 != null)
                    {
                        ThisBettingLine.AwayTeamAbbreviation = prop_teamShortName1.Value.ToString();
                    }

                    //Team 2 short name
                    JProperty? prop_teamShortName2 = e.Property("teamShortName2");
                    if (prop_teamShortName2 != null)
                    {
                        ThisBettingLine.HomeTeamAbbreviation = prop_teamShortName2.Value.ToString();
                    }

                    //Add
                    ToReturn.Add(ThisBettingLine);
                }
            }

            return ToReturn.ToArray();


        }
    }
}