using System;

namespace ESPN
{
    public class Game
    {
        public float AwayTeamWinningRecord {get; set;}
        public float HomeTeamWinningRecord {get; set;}
        public int AwayRuns {get; set;}
        public int HomeRuns {get; set;}
        public int Inning {get; set;}
        public Team BattingTeam {get; set;}
        public int Outs {get; set;}
        public int Balls {get; set;}
        public int Strikes {get; set;}
        public bool ManOnFirst {get; set;}
        public bool ManOnSecond {get; set;}
        public bool ManOnThird {get; set;}
    }
}