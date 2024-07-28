using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CupManagerUI : MonoBehaviour, ICupPresenter
{
    public TextMeshProUGUI CupLabel;
    public TextMeshProUGUI TeamList;
    public TextMeshProUGUI CupPhaseLabel;
    public TextMeshProUGUI MatchList;

    public void DisplayCup(Cup cup)
    {
        CupLabel.text = $"{cup.Name}";
    }

    public void DisplayTeams(List<Team> teams)
    {
        TeamList.text = string.Join("\n", teams.Select(x => x.Name));
    }

    public void DisplayRound(Round round)
    {
        CupPhaseLabel.text = round.Phase.ToString();

        List<string> matchesText = new();
        foreach (var match in round.Matches)
        {
            var matchText = $"{match.HomeTeam} {match.MatchResult?.HomeTeamGoals} x {match.MatchResult?.AwayTeamGoals} {match.AwayTeam}";
            matchesText.Add(matchText);
        }
        MatchList.text = string.Join("\n", matchesText);
    }
}
