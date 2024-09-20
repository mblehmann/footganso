using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeagueManagerUI : MonoBehaviour, ILeaguePresenter
{
    public TextMeshProUGUI LeagueLabel;
    public TextMeshProUGUI Standings;
    public TextMeshProUGUI LeagueRoundLabel;
    public TextMeshProUGUI MatchList;

    public void DisplayLeague(League league)
    {
        LeagueLabel.text = $"{league.Name}";
    }

    public void DisplayStandings(Dictionary<Team, int> standings)
    {
        Standings.text = string.Join("\n", standings.OrderByDescending(x => x.Value).Select(x => $"{x.Key.Name}: {x.Value}"));
    }

    public void DisplayRound(LeagueRound round)
    {
        LeagueRoundLabel.text = $"Round {round.Index}";

        List<string> matchesText = new();
        foreach (var match in round.Matches)
        {
            var matchText = $"{match.HomeTeam} {match.MatchResult?.HomeTeamGoals} x {match.MatchResult?.AwayTeamGoals} {match.AwayTeam}";
            matchesText.Add(matchText);
        }
        MatchList.text = string.Join("\n", matchesText);
    }
}
