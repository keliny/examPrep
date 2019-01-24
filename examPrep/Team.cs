namespace examPrep
{
    class Team
    {
        public string Name { get; set; }
        public int GoalsGivenHome { get; set; }
        public int GoalsGivenAway { get; set; }
        public int GoalsTakenHome { get; set; }
        public int GoalsTakenAway { get; set; }
        public int Points { get; set; }

        public Team(string name)
        {
            Name = name;
            GoalsGivenHome = 0;
            GoalsGivenAway = 0;
            GoalsTakenAway = 0;
            GoalsTakenHome = 0;
            Points = 0;
        }
    }
}