using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace examPrep
{
    class Program
    {
        public static string Data { get; set; }
        public static string Commands { get; set; }
        public static string Output { get; set; }
        public static List<Match> Matches { get; set; } = new List<Match>();
        public static Dictionary<string, Team> Teams { get; set; } = new Dictionary<string, Team>();
        public static List<int> Range { get; set; } = new List<int>();

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                throw new SystemException("Parameters are not equal to 3.");
            }

            Data = args[0];
            Commands = args[1];
            Output = args[2];

            CheckFiles();
            AssignMatches();
            GetRounds();
        }

        public static void CheckFiles()
        {
            if (!File.Exists(Data) || !File.Exists(Commands))
            {
                throw new SystemException("One of the files does not exist.");
            }
        }

        public static void AssignMatches()
        {
            var lines = File.ReadAllLines(Data);

            // Load Teams and Matches
            foreach (var l in lines)
            {
                var line = l.Split(',');
                var dateData = line[0].Split('.');
                var date = new DateTime(int.Parse(dateData[2]), int.Parse(dateData[1]), int.Parse(dateData[0])); // date has incorrect format
                //Create team
                // check for existing teams, create them
                if (!Teams.ContainsKey(line[2]))
                {
                    Teams.Add(line[2], new Team(line[2]));
                }

                if (!Teams.ContainsKey(line[3]))
                {
                    Teams.Add(line[3], new Team(line[3]));
                }

                // assign goals
                var homeTeam = Teams.FirstOrDefault(x => x.Key == line[2]);
                var awayTeam = Teams.FirstOrDefault(x => x.Key == line[3]);

                var goalsHome = int.Parse(line[4].Split(':')[0]);
                var goalsAway = int.Parse(line[4].Split(':')[1].Split('(')[0]);

                homeTeam.Value.GoalsGivenHome += goalsHome;
                homeTeam.Value.GoalsTakenHome += goalsAway;

                awayTeam.Value.GoalsGivenAway += goalsAway;
                awayTeam.Value.GoalsTakenAway += goalsHome;

                // calculate points - normal win 3p, PP/SN win 2p, PP/SN lose 1p, normal lose 0p
                if (goalsHome > goalsAway)
                {
                    if (line[4].Contains("PP") || line[4].Contains("SN"))
                    {
                        homeTeam.Value.Points += 2;
                        awayTeam.Value.Points += 1;
                    }
                    else
                    {
                        homeTeam.Value.Points += 3;
                    }
                }
                else
                {
                    if (line[4].Contains("PP") || line[4].Contains("SN"))
                    {
                        homeTeam.Value.Points += 1;
                        awayTeam.Value.Points += 2;
                    }
                    else
                    {
                        awayTeam.Value.Points += 3;
                    }
                }

                // create Match
                var type = "N";
                if (line[4].Contains("PP"))
                {
                    type = "PP";
                }
                else if (line[4].Contains("SN"))
                {
                    type = "SN";
                }

                Matches.Add(new Match(date, int.Parse(line[1]), line[2], line[3], int.Parse(line[4].Split(':')[0]), int.Parse(line[4].Split(':')[1].Split('(')[0]), type));
            }

        }

        public static void GetRounds()
        {
            var doc = new XmlDocument();
            doc.Load(Commands);
            var rounds = doc.DocumentElement.SelectNodes("/hockey/rounds");
            var range = doc.DocumentElement.SelectNodes("/hockey/rounds/range");
            var statistics = doc.DocumentElement.SelectNodes("/hockey/statistics");

            foreach (XmlNode round in rounds)
            {
                Console.WriteLine(round.SelectNodes("round"));
            }

            // fill range with data
            foreach (XmlNode rang in range)
            {
                foreach (XmlElement r in rang)
                {
                    Range.Add(int.Parse(r.InnerText));
                }
            }

            foreach (XmlNode stat in statistics)
            {
                foreach (XmlElement st in stat)
                {
                    Console.WriteLine(st.Attributes["num"].Value);
                    Console.WriteLine(st.Attributes["type"].Value);
                }
                Console.WriteLine(stat.Value);
            }
        }
    }
}