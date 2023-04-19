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
        public int AwayTeamMoneyLine {get; set;} //i.e. -220
        public int HomeTeamMoneyLine {get; set;} //i.e. +180

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


            //Now get the offers (lines) available
            JToken? offers = root.SelectToken("offers.84240");
            if (offers != null)
            {
                JObject _offers = (JObject)offers;
                foreach (JProperty prop in _offers.Properties())
                {
                    JObject this_offer = (JObject)prop.Value;

                    //Get the Id
                    int event_id = 0;
                    JProperty? prop_eventId = this_offer.Property("eventId");
                    if (prop_eventId != null)
                    {
                        event_id = Convert.ToInt32(prop_eventId.Value.ToString());
                    }

                    //Find which event this is far
                    BettingLine? target_event = null;
                    foreach (BettingLine bl in ToReturn)
                    {
                        if (bl.Id == event_id)
                        {
                            target_event = bl;
                        }
                    }

                    //If we found a matching target event
                    if (target_event != null)
                    {

                        //get the label
                        string label = "";
                        JProperty? prop_label = this_offer.Property("label");
                        if (prop_label != null)
                        {
                            label = prop_label.Value.ToString();
                        }

                        if (label.ToLower() == "run line")
                        {
                            JToken? run_line = this_offer.SelectToken("outcomes[0].line");
                            if (run_line != null)
                            {
                                target_event.RunLine = Math.Abs(Convert.ToSingle(run_line.ToString()));
                            }
                        }
                        else if (label.ToLower() == "total")
                        {
                            JToken? total_line = this_offer.SelectToken("outcomes[0].line");
                            if (total_line != null)
                            {
                                target_event.TotalLine = Math.Abs(Convert.ToSingle(total_line.ToString()));
                            }
                        }
                        else if (label.ToLower() == "moneyline")
                        {
                            JToken? outcome0label = this_offer.SelectToken("outcomes[0].label");
                            JToken? outcome0odds = this_offer.SelectToken("outcomes[0].oddsAmerican");
                            JToken? outcome1label = this_offer.SelectToken("outcomes[1].label");
                            JToken? outcome1odds = this_offer.SelectToken("outcomes[1].oddsAmerican");

                            if (outcome0label != null && outcome0odds != null && outcome1label != null && outcome1odds != null)
                            {
                                int line0 = Convert.ToInt32(outcome0odds.ToString().Replace("−", "-").Replace("+", ""));
                                int line1 = Convert.ToInt32(outcome1odds.ToString().Replace("−", "-").Replace("+", ""));
                                string team0 = outcome0label.ToString();
                                string team1 = outcome1label.ToString();

                                //Put in correct place - line 0
                                if (team0.ToLower() == target_event.AwayTeam.ToLower())
                                {
                                    target_event.AwayTeamMoneyLine = line0;
                                }
                                else if (team0.ToLower() == target_event.HomeTeam.ToLower())
                                {
                                    target_event.HomeTeamMoneyLine = line0;
                                }

                                //Put in correct place - line 1
                                if (team1.ToLower() == target_event.AwayTeam.ToLower())
                                {
                                    target_event.AwayTeamMoneyLine = line1;
                                }
                                else if (team1.ToLower() == target_event.HomeTeam.ToLower())
                                {
                                    target_event.HomeTeamMoneyLine = line1;
                                }
                            }
                        }


                    }

                }
            }


            return ToReturn.ToArray();


        }
    
        public float[] ToState()
        {
            List<float> ToReturn = new List<float>();
            ToReturn.Add(RunLine);
            ToReturn.Add(TotalLine);
            ToReturn.Add(Convert.ToSingle(AwayTeamMoneyLine));
            ToReturn.Add(Convert.ToSingle(HomeTeamMoneyLine));
            return ToReturn.ToArray();
        }

    }
}