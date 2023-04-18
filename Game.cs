using System;

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