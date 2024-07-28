using System;
using System.Collections.Generic;
using System.Linq;

public class CupInteractor
{
    private readonly IShuffler Shuffler;
    private readonly ICompetitionFactory CupFactory;
    private readonly ICupPresenter CupPresenter;

    public CupInteractor(IShuffler shuffler, ICompetitionFactory cupFactory, ICupPresenter cupPresenter)
    {
        Shuffler = shuffler;
        CupFactory = cupFactory;
        CupPresenter = cupPresenter;
    }

    public Cup CreateCup(string cupName)
    {
        Cup cup = CupFactory.CreateCup(cupName);
        CupPresenter?.DisplayCup(cup);
        return cup;
    }

    public void AddTeams(Cup cup, List<Team> teams)
    {
        cup.AddTeams(teams);
        CupPresenter?.DisplayTeams(cup.Teams);
    }

    public void DrawRound(Cup cup)
    {
        Round currentRound = CreateRound(cup);
        if (currentRound.Phase == CupPhase.Over) return;

        cup.AddRound(currentRound);
        CupPresenter?.DisplayRound(cup.CurrentRound);
    }

    private Round CreateRound(Cup cup)
    {
        var cupPhase = GetCupPhase(cup.Teams.Count);
        Round currentRound = CupFactory.CreateRound(cupPhase);
        if (cupPhase == CupPhase.Over) return currentRound;

        List<Match> matches = DrawMatches(cup, cupPhase);
        currentRound.AddMatches(matches);
        return currentRound;
    }

    private List<Match> DrawMatches(Cup cup, CupPhase currentPhase)
    {
        List<Match> matches = new();
        int teamsInNextRound = (int)currentPhase / 2;
        int numberOfMatches = cup.Teams.Count - teamsInNextRound;
        var shuffledTeams = Shuffler.ShuffleTeams(cup.Teams).GetEnumerator();

        for (int i = 0; i < numberOfMatches; i++)
        {
            var matchId = cup.GetNextMatchId();
            var homeTeam = GetNextTeam(shuffledTeams);
            var awayTeam = GetNextTeam(shuffledTeams);

            Match match = CupFactory.CreateMatch(matchId, homeTeam, awayTeam);
            matches.Add(match);
        }

        return matches;
    }

    public Round GetCurrentRound(Cup cup)
    {
        return cup.CurrentRound;
    }

    public void UpdateScores(Cup cup, List<MatchResult> matchResults)
    {
        var currentRound = GetCurrentRound(cup);

        foreach (var result in matchResults)
        {
            var match = currentRound.Matches.SingleOrDefault(x => x.Id == result.MatchId);
            match?.SetResult(result);
        }

        CupPresenter?.DisplayRound(cup.CurrentRound);
    }

    public void FinishRound(Cup cup)
    {
        cup.RemoveTeams(cup.CurrentRound.Losers);
        CupPresenter?.DisplayTeams(cup.Teams);
    }

    public bool IsOver(Cup cup)
    {
        return cup.Teams.Count == 1;
    }

    public Team GetWinner(Cup cup)
    {
        if (IsOver(cup)) return cup.Teams.Single();
        return null;
    }

    // Progressing Rounds: Move the cup to the next round based on match results.
    // Determining Cup Winner: Conclude the cup and determine the final winner.

    private CupPhase GetCupPhase(int numberOfTeams)
    {
        if (numberOfTeams == 1) return CupPhase.Over;
        if (numberOfTeams == 2) return CupPhase.Final;
        if (numberOfTeams <= 4) return CupPhase.SemiFinal;
        if (numberOfTeams <= 8) return CupPhase.QuarterFinal;
        if (numberOfTeams <= 16) return CupPhase.RoundOf16;
        if (numberOfTeams <= 32) return CupPhase.RoundOf32;
        if (numberOfTeams <= 64) return CupPhase.RoundOf64;
        if (numberOfTeams <= 128) return CupPhase.RoundOf128;
        return CupPhase.None;
    }

    private Team GetNextTeam(IEnumerator<Team> teams)
    {
        teams.MoveNext();
        return teams.Current;
    }
}