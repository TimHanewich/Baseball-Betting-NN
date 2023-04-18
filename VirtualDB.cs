using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ESPN
{
    public class VirtualDB
    {
        private string path; //Path to the .jsonl file, where each line is a StateProbabilityPair in JSON

        public VirtualDB(string jsonl_path)
        {
            path = jsonl_path; 
        }

        //Sees if we already have this EXACT state and it's implied probability on file
        public bool Stored(float[] state)
        {
            StreamReader sr = new StreamReader(path);
            
            bool stop = false;
            while (stop == false)
            {
                string? line = sr.ReadLine();
                if (line == null)
                {
                    stop = true;
                }
                else
                {
                    StateProbabilityPair? spp = JsonConvert.DeserializeObject<StateProbabilityPair>(line);
                    if (spp != null)
                    {
                        if (Game.EquivalentStates(spp.State, state))
                        {
                            sr.Close();
                            return true;
                        }
                    }
                }
            }

            //it got this far, so no, we don't have it yet.
            sr.Close();
            return false;
        }
    
        public void Add(StateProbabilityPair spp)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(JsonConvert.SerializeObject(spp, Formatting.None));
            sw.Close();
        }

        public void AddIfNotStored(StateProbabilityPair spp)
        {
            bool exists = Stored(spp.State);
            if (exists == false)
            {
                Add(spp);
            }
        }

        public StateProbabilityPair[] RetrieveAll()
        {
            List<StateProbabilityPair> ToReturn = new List<StateProbabilityPair>();
            StreamReader sr = new StreamReader(path);
            
            bool stop = false;
            while (stop == false)
            {
                string? line = sr.ReadLine();
                if (line == null)
                {
                    stop = true;
                }
                else
                {
                    StateProbabilityPair? spp = JsonConvert.DeserializeObject<StateProbabilityPair>(line);
                    if (spp != null)
                    {
                        ToReturn.Add(spp);
                    }
                }
            }

            sr.Close();
            return ToReturn.ToArray();
        }

    }
}