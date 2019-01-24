using System;

namespace examPrep
{
    public class Match
    {
        public DateTime Date { get; set; }
        public int RoundNumber { get; set; }
        public string HomeTeamName { get; set; }
        public string HostTeamName { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public string WinType { get; set; }

        public Match(DateTime date, int roundNumber, string homeTeam, string hostTeam, int homeTeamGoals, int awayTeamGoals, string winType)
        {
            Date = date;
            RoundNumber = roundNumber;
            HomeTeamName = homeTeam;
            HostTeamName = hostTeam;
            AwayTeamGoals = awayTeamGoals;
            HomeTeamGoals = homeTeamGoals;
            WinType = winType;
        }
    }
}