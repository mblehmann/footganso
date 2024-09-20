using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LeagueTest
{
    private IShuffler Shuffler;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Shuffler = Substitute.For<IShuffler>();
        Shuffler.ShuffleTeams(Arg.Any<List<Team>>()).Returns(x => x[0]);
    }


    [Test]
    public void LeagueCreationTest()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        string leagueName = "League Name";

        League league = leagueInteractor.CreateLeague(leagueName);

        Assert.AreEqual(leagueName, league.Name);
        Assert.IsEmpty(league.Teams);
    }

    [TestCase(8)]
    [TestCase(20)]
    public void AddTeamsTest(int numberOfTeams)
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(numberOfTeams);

        leagueInteractor.AddTeams(league, leagueTeams);

        Assert.AreEqual(leagueTeams, league.Teams);
        Assert.AreEqual(leagueTeams, league.Standings.Keys);
    }

    [Test]
    public void RemoveTeamsTest()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(8);
        List<Team> relegatedTeams = new(){ leagueTeams.First(), leagueTeams.Last() };
        List<Team> remainingTeams = leagueTeams.Except(relegatedTeams).ToList();
        leagueInteractor.AddTeams(league, leagueTeams);

        leagueInteractor.RemoveTeams(league, relegatedTeams);

        Assert.AreEqual(remainingTeams, league.Teams);
        Assert.AreEqual(remainingTeams, league.Standings.Keys);
    }

    [Test]
    public void StandingsTest()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(8);
        leagueInteractor.AddTeams(league, leagueTeams);

        foreach (var team in leagueTeams)
        {
            Assert.AreEqual(0, league.Standings[team]);
        }
    }

    [Test]
    public void DrawLeagueSchedule4TeamsTest()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(4);
        leagueInteractor.AddTeams(league, leagueTeams);

        List<List<Match>> matches = new()
        {
            new() { new(0, leagueTeams[3], leagueTeams[0]), new(1, leagueTeams[1], leagueTeams[2]) },
            new() { new(2, leagueTeams[0], leagueTeams[2]), new(3, leagueTeams[3], leagueTeams[1]) },
            new() { new(4, leagueTeams[1], leagueTeams[0]), new(5, leagueTeams[2], leagueTeams[3]) },

            new() { new(6, leagueTeams[0], leagueTeams[3]), new(7, leagueTeams[2], leagueTeams[1]) },
            new() { new(8, leagueTeams[2], leagueTeams[0]), new(9, leagueTeams[1], leagueTeams[3]) },
            new() { new(10, leagueTeams[0], leagueTeams[1]), new(11, leagueTeams[3], leagueTeams[2]) },
        };
        
        leagueInteractor.DrawSchedule(league);

        var home_matches = new Dictionary<string, int>();
        var away_matches = new Dictionary<string, int>();
        foreach (var round in league.Rounds)
        {
            Assert.AreEqual(matches[round.Index], round.Matches);
            foreach (var match in round.Matches)
            {
                if (!home_matches.ContainsKey(match.HomeTeam.Name))
                {
                    home_matches[match.HomeTeam.Name] = 0;
                }
                if (!away_matches.ContainsKey(match.AwayTeam.Name))
                {
                    away_matches[match.AwayTeam.Name] = 0;
                }
                home_matches[match.HomeTeam.Name]++;
                away_matches[match.AwayTeam.Name]++;
            }
        }

        foreach (var entry in home_matches)
        {
            Assert.AreEqual(3, entry.Value);
        }

        foreach (var entry in away_matches)
        {
            Assert.AreEqual(3, entry.Value);
        }

        Assert.AreEqual(0, league.CurrentRound.Index);
    }

    [Test]
    public void DrawLeagueScheduleTest()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(8);
        leagueInteractor.AddTeams(league, leagueTeams);

        List<List<Match>> matches = new()
        {
            new() { new(0, leagueTeams[7], leagueTeams[0]), new(1, leagueTeams[1], leagueTeams[6]), new(2, leagueTeams[2], leagueTeams[5]), new(3, leagueTeams[3], leagueTeams[4]) },
            new() { new(4, leagueTeams[0], leagueTeams[4]), new(5, leagueTeams[5], leagueTeams[3]), new(6, leagueTeams[6], leagueTeams[2]), new(7, leagueTeams[7], leagueTeams[1]) },
            new() { new(8, leagueTeams[1], leagueTeams[0]), new(9, leagueTeams[2], leagueTeams[7]), new(10, leagueTeams[3], leagueTeams[6]), new(11, leagueTeams[4], leagueTeams[5]) },
            new() { new(12, leagueTeams[0], leagueTeams[5]), new(13, leagueTeams[6], leagueTeams[4]), new(14, leagueTeams[7], leagueTeams[3]), new(15, leagueTeams[1], leagueTeams[2]) },
            new() { new(16, leagueTeams[2], leagueTeams[0]), new(17, leagueTeams[3], leagueTeams[1]), new(18, leagueTeams[4], leagueTeams[7]), new(19, leagueTeams[5], leagueTeams[6]) },
            new() { new(20, leagueTeams[0], leagueTeams[6]), new(21, leagueTeams[7], leagueTeams[5]), new(22, leagueTeams[1], leagueTeams[4]), new(23, leagueTeams[2], leagueTeams[3]) },
            new() { new(24, leagueTeams[3], leagueTeams[0]), new(25, leagueTeams[4], leagueTeams[2]), new(26, leagueTeams[5], leagueTeams[1]), new(27, leagueTeams[6], leagueTeams[7]) },

            new() { new(28, leagueTeams[0], leagueTeams[7]), new(29, leagueTeams[6], leagueTeams[1]), new(30, leagueTeams[5], leagueTeams[2]), new(31, leagueTeams[4], leagueTeams[3]) },
            new() { new(32, leagueTeams[4], leagueTeams[0]), new(33, leagueTeams[3], leagueTeams[5]), new(34, leagueTeams[2], leagueTeams[6]), new(35, leagueTeams[1], leagueTeams[7]) },
            new() { new(36, leagueTeams[0], leagueTeams[1]), new(37, leagueTeams[7], leagueTeams[2]), new(38, leagueTeams[6], leagueTeams[3]), new(39, leagueTeams[5], leagueTeams[4]) },
            new() { new(40, leagueTeams[5], leagueTeams[0]), new(41, leagueTeams[4], leagueTeams[6]), new(42, leagueTeams[3], leagueTeams[7]), new(43, leagueTeams[2], leagueTeams[1]) },
            new() { new(44, leagueTeams[0], leagueTeams[2]), new(45, leagueTeams[1], leagueTeams[3]), new(46, leagueTeams[7], leagueTeams[4]), new(47, leagueTeams[6], leagueTeams[5]) },
            new() { new(48, leagueTeams[6], leagueTeams[0]), new(49, leagueTeams[5], leagueTeams[7]), new(50, leagueTeams[4], leagueTeams[1]), new(51, leagueTeams[3], leagueTeams[2]) },
            new() { new(52, leagueTeams[0], leagueTeams[3]), new(53, leagueTeams[2], leagueTeams[4]), new(54, leagueTeams[1], leagueTeams[5]), new(55, leagueTeams[7], leagueTeams[6]) },
        };
        
        leagueInteractor.DrawSchedule(league);

        var home_matches = new Dictionary<string, int>();
        var away_matches = new Dictionary<string, int>();
        foreach (var round in league.Rounds)
        {
            Assert.AreEqual(matches[round.Index], round.Matches);
            foreach (var match in round.Matches)
            {
                if (!home_matches.ContainsKey(match.HomeTeam.Name))
                {
                    home_matches[match.HomeTeam.Name] = 0;
                }
                if (!away_matches.ContainsKey(match.AwayTeam.Name))
                {
                    away_matches[match.AwayTeam.Name] = 0;
                }
                home_matches[match.HomeTeam.Name]++;
                away_matches[match.AwayTeam.Name]++;
            }
        }

        foreach (var entry in home_matches)
        {
            Assert.AreEqual(7, entry.Value);
        }

        foreach (var entry in away_matches)
        {
            Assert.AreEqual(7, entry.Value);
        }

        Assert.AreEqual(0, league.CurrentRound.Index);
    }

    [Test]
    public void UpdateScoresOfRoundTest()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(8);
        leagueInteractor.AddTeams(league, leagueTeams);
        leagueInteractor.DrawSchedule(league);

        Dictionary<Team, int> expectedPoints = new()
        {
            { leagueTeams[0], 0 },
            { leagueTeams[1], 0 },
            { leagueTeams[2], 1 },
            { leagueTeams[3], 1 },
            { leagueTeams[4], 1 },
            { leagueTeams[5], 1 },
            { leagueTeams[6], 3 },
            { leagueTeams[7], 3 },
        };
        List<MatchResult> matchResults = new()
        {
            new MatchResult(0, 1, 0),
            new MatchResult(1, 1, 3),
            new MatchResult(2, 2, 2),
            new MatchResult(3, 0, 0)
        };

        leagueInteractor.UpdateScores(league, matchResults);

        Assert.AreEqual(expectedPoints, league.Standings);
    }

    [Test]
    public void AdvanceRoundTest()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(8);
        leagueInteractor.AddTeams(league, leagueTeams);
        leagueInteractor.DrawSchedule(league);
        List<MatchResult> matchResults = new()
        {
            new MatchResult(0, 1, 0),
            new MatchResult(1, 1, 3),
            new MatchResult(2, 2, 2),
            new MatchResult(3, 0, 0)
        };

        leagueInteractor.UpdateScores(league, matchResults);
        leagueInteractor.FinishRound(league);

        Assert.AreEqual(1, league.CurrentRound.Index);
    }

    [Test]
    public void PlayEntireLeague()
    {
        LeagueInteractor leagueInteractor = new(Shuffler, null);
        League league = leagueInteractor.CreateLeague("League Name");
        List<Team> leagueTeams = CreateTeams(8);
        leagueInteractor.AddTeams(league, leagueTeams);
        leagueInteractor.DrawSchedule(league);
        Dictionary<Team, int> expectedResult = new()
        {
            { leagueTeams[0], 42 },
            { leagueTeams[1], 36 },
            { leagueTeams[2], 30 },
            { leagueTeams[3], 24 },
            { leagueTeams[4], 18 },
            { leagueTeams[5], 12 },
            { leagueTeams[6], 6 },
            { leagueTeams[7], 0 },
        };

        while (!leagueInteractor.IsOver(league))
        {
            List<MatchResult> matchResults = new();
            LeagueRound currentRound = league.CurrentRound;
            foreach (var match in currentRound.Matches)
            {
                if (match.HomeTeam.Name.CompareTo(match.AwayTeam.Name) < 0)
                {
                    matchResults.Add(new MatchResult(match.Id, 1, 0));
                }
                else
                {
                    matchResults.Add(new MatchResult(match.Id, 0, 1));
                }
            }
            leagueInteractor.UpdateScores(league, matchResults);
            leagueInteractor.FinishRound(league);
        }

        
        Assert.AreEqual(expectedResult, league.Standings);
    }

    private List<Team> CreateTeams(int numberOfTeams, int startIndex = 0)
    {
        List<Team> teams = new();

        for (int i = 0; i < numberOfTeams; i++)
        {
            teams.Add(new Team($"Team-{startIndex+i}"));
        }

        return teams;
    }
}
