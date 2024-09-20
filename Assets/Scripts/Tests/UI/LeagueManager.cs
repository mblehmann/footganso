using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeagueManager : MonoBehaviour
{
    private LeagueInteractor LeagueInteractor;

    public string LeagueName;
    public int NumberOfTeams;
    public float HomeWinPercentage;

    private League League;

    // Start is called before the first frame update
    void Start()
    {
        LeagueManagerUI leagueDisplayer = GetComponent<LeagueManagerUI>();
        LeagueInteractor = new LeagueInteractor(new Shuffler(), leagueDisplayer); //, new CupFactory(), cupDisplayer);
    }
    
    public void CreateLeague()
    {
        League = LeagueInteractor.CreateLeague(LeagueName);
        List<Team> teams = new();
        for (int i = 0; i < NumberOfTeams; i++)
            teams.Add(new Team($"Team-{i}"));
        LeagueInteractor.AddTeams(League, teams);
        LeagueInteractor.DrawSchedule(League);
    }

    public void SimulateMatches()
    {
        List<MatchResult> matchResults = new();
        LeagueRound currentRound = League.CurrentRound;
        foreach (var match in currentRound.Matches)
        {
            if (Random.Range(0f, 1f) <= HomeWinPercentage)
            {
                matchResults.Add(new MatchResult(match.Id, 1, 0));
            }
            else
            {
                matchResults.Add(new MatchResult(match.Id, 0, 1));
            }
        }

        LeagueInteractor.UpdateScores(League, matchResults);
    }

    public void AdvanceRound()
    {
        LeagueInteractor.FinishRound(League);
    }
}
