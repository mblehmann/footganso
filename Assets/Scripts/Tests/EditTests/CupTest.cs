using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;

public class CupTest
{
    private IShuffler Shuffler;
    private ICompetitionFactory CupFactory;
    private ICupPresenter CupPresenter;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Shuffler = Substitute.For<IShuffler>();
        Shuffler.ShuffleTeams(Arg.Any<List<Team>>()).Returns(x => x[0]);

        CupFactory = new CupFactory();        
    }

    [Test]
    public void CupCreationTest()
    {
        CupInteractor cupInteractor = new(Shuffler, CupFactory, null);
        string cupName = "Cup Name";

        Cup cup = cupInteractor.CreateCup(cupName);

        Assert.AreEqual(cupName, cup.Name);
        Assert.IsEmpty(cup.Teams);
    }

    [TestCase(1)]
    [TestCase(3)]
    public void AddTeamsTest(int numberOfTeams)
    {
        CupInteractor cupInteractor = new(Shuffler, CupFactory, null);
        Cup cup = cupInteractor.CreateCup("Cup Name");
        List<Team> cupTeams = CreateTeams(numberOfTeams);

        cupInteractor.AddTeams(cup, cupTeams);

        Assert.AreEqual(cupTeams, cup.Teams);
    }

    [Test]
    public void AddTeamsFromMultipleLeaguesTest()
    {
        CupInteractor cupInteractor = new(Shuffler, CupFactory, null);
        Cup cup = cupInteractor.CreateCup("Cup Name");

        Team team1 = new("Team1");
        Team team2 = new("Team2");
        Team teamA = new("TeamA");
        Team teamB = new("TeamB");

        List<Team> leagueTeams = new() { team1, team2 };
        List<Team> regionalTeams = new() { teamA, teamB };
        List<Team> allTeams = new () { team1, team2, teamA, teamB };

        cupInteractor.AddTeams(cup, leagueTeams);
        cupInteractor.AddTeams(cup, regionalTeams);

        Assert.AreEqual(allTeams, cup.Teams);
    }

    [TestCase(CupPhase.SemiFinal, 2, 4)]
    [TestCase(CupPhase.QuarterFinal, 3, 7)]
    [TestCase(CupPhase.RoundOf16, 1, 9)]
    public void DrawCupRoundTest(CupPhase expectedCupPhase, int expectedNumberOfMatches, int numberOfTeams)
    {
        CupInteractor cupInteractor = new(Shuffler, CupFactory, null);
        Cup cup = cupInteractor.CreateCup("Cup Name");
        List<Team> cupTeams = CreateTeams(numberOfTeams);
        cupInteractor.AddTeams(cup, cupTeams);

        cupInteractor.DrawRound(cup);

        Round currentRound = cupInteractor.GetCurrentRound(cup);
        Assert.AreEqual(expectedCupPhase, currentRound.Phase);
        var matches = currentRound.Matches;
        Assert.AreEqual(expectedNumberOfMatches, matches.Count);
        for (int i = 0; i < expectedNumberOfMatches; i++)
        {
            var match = matches[i];
            Assert.AreEqual(i, match.Id);
            Assert.AreEqual(cupTeams[i*2], match.HomeTeam);
            Assert.AreEqual(cupTeams[i*2+1], match.AwayTeam);
        }
    }

    [Test]
    public void UpdateScoresWinnerInRegularTimeTest()
    {
        CupInteractor cupInteractor = new(Shuffler, CupFactory, null);
        Cup cup = cupInteractor.CreateCup("Cup Name");
        List<Team> cupTeams = CreateTeams(4);
        cupInteractor.AddTeams(cup, cupTeams);
        cupInteractor.DrawRound(cup);

        List<Team> expectedWinners = new() { cupTeams[0], cupTeams[3] };
        List<MatchResult> matchResults = new()
        {
            new MatchResult(0, 1, 0),
            new MatchResult(1, 1, 3)
        };

        cupInteractor.UpdateScores(cup, matchResults);

        Round currentRound = cupInteractor.GetCurrentRound(cup);
        Assert.AreEqual(expectedWinners, currentRound.Winners);
    }

    [TestCase(4)]
    [TestCase(7)]
    [TestCase(9)]
    public void FinishRoundTest(int numberOfTeams)
    {
        CupInteractor cupInteractor = new(Shuffler, CupFactory, null);
        Cup cup = cupInteractor.CreateCup("Cup Name");
        List<Team> cupTeams = CreateTeams(numberOfTeams);
        cupInteractor.AddTeams(cup, cupTeams);
        cupInteractor.DrawRound(cup);

        List<MatchResult> matchResults = new();
        List<Team> remainingTeams = new(cup.Teams);
        Round currentRound = cupInteractor.GetCurrentRound(cup);
        foreach (var match in currentRound.Matches)
        {
            matchResults.Add(new MatchResult(match.Id, 1, 0));
            remainingTeams.Remove(match.AwayTeam);
        }
        cupInteractor.UpdateScores(cup, matchResults);

        cupInteractor.FinishRound(cup);
        
        Assert.AreEqual(remainingTeams, cup.Teams);
    }

    [TestCase(2)]
    [TestCase(3)]
    [TestCase(9)]
    [TestCase(128)]
    public void PlayEntireCupTest(int numberOfTeams)
    {
        CupInteractor cupInteractor = new(Shuffler, CupFactory, null);
        Cup cup = cupInteractor.CreateCup("Cup Name");
        List<Team> cupTeams = CreateTeams(numberOfTeams);
        cupInteractor.AddTeams(cup, cupTeams);
        Team expectedWinner = cupTeams[0];

        while (!cupInteractor.IsOver(cup))
        {
            cupInteractor.DrawRound(cup);
            List<MatchResult> matchResults = new();
            Round currentRound = cupInteractor.GetCurrentRound(cup);
            foreach (var match in currentRound.Matches)
            {
                matchResults.Add(new MatchResult(match.Id, 1, 0));
            }
            cupInteractor.UpdateScores(cup, matchResults);
            cupInteractor.FinishRound(cup);
        }

        
        Assert.AreEqual(expectedWinner, cupInteractor.GetWinner(cup));
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
