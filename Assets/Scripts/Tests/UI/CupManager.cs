using System.Collections.Generic;
using UnityEngine;

public class CupManager : MonoBehaviour
{
    private CupInteractor CupInteractor;

    public string CupName;
    public int NumberOfTeams;
    public float HomeWinPercentage;

    private Cup Cup;

    // Start is called before the first frame update
    void Start()
    {
        CupManagerUI cupDisplayer = GetComponent<CupManagerUI>();
        CupInteractor = new CupInteractor(new Shuffler(), new CupFactory(), cupDisplayer);
    }
    
    public void CreateCup()
    {
        Cup = CupInteractor.CreateCup(CupName);
        List<Team> teams = new();
        for (int i = 0; i < NumberOfTeams; i++)
            teams.Add(new Team($"Team-{i}"));
        CupInteractor.AddTeams(Cup, teams);
    }

    public void DrawRound()
    {
        CupInteractor.DrawRound(Cup);
    }

    public void SimulateMatches()
    {
        List<MatchResult> matchResults = new();
        Round currentRound = CupInteractor.GetCurrentRound(Cup);
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

        CupInteractor.UpdateScores(Cup, matchResults);
    }

    public void AdvanceRound()
    {
        CupInteractor.FinishRound(Cup);
    }

    // public string GetCupName()
    // {
    //     return Cup?.Name ?? string.Empty;
    // }

    // public List<Team> GetRemainingTeams()
    // {
    //     return Cup?.Teams ?? new List<Team>();
    // }

    // public string GetCupPhase()
    // {
    //     return Cup?.CurrentRound?.Phase.ToString() ?? string.Empty;
    // }

    // public List<Match> GetCurrentMatches()
    // {
    //     return Cup?.CurrentRound?.Matches ?? new List<Match>();
    // }
}
