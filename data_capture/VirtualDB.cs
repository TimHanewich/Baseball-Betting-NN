using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ESPN
{
    public class VirtualDB
    {
        private string path; //Path to the .jsonl file, where each line is a StatePredictionPair in JSON

        public VirtualDB(string jsonl_path)
        {
            path = jsonl_path; 
        }

        //Sees if we already have this EXACT state and it's implied probability on file
        public bool Stored(StatePredictionPair spp)
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
                    StatePredictionPair? spp_ = JsonConvert.DeserializeObject<StatePredictionPair>(line);
                    if (spp_ != null)
                    {
                        if (JsonConvert.SerializeObject(spp_) == JsonConvert.SerializeObject(spp)) //If it is not EXACTLY the same
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
    
        public void Add(StatePredictionPair spp)
        {
            StreamWriter sw = System.IO.File.AppendText(path);
            sw.WriteLine(JsonConvert.SerializeObject(spp, Formatting.None));
            sw.Close();
        }


        //Returns true if added, false if not
        public bool AddIfNotStored(StatePredictionPair spp)
        {
            bool exists = Stored(spp);
            if (exists == false)
            {
                Add(spp);
                return true;
            }
            else
            {
                return false;
            }
        }

        public StatePredictionPair[] RetrieveAll()
        {
            List<StatePredictionPair> ToReturn = new List<StatePredictionPair>();
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
                    StatePredictionPair? spp = JsonConvert.DeserializeObject<StatePredictionPair>(line);
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