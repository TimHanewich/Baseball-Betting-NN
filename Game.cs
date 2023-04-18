using System;
using System.Collections;
using System.Collections.Generic;

namespace ESPN
{
    public class Game
    {
        public int Id {get; set;}
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
    }
}