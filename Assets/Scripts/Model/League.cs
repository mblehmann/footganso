using System.Collections.Generic;
using System.Linq;

public class League
{
    private int MatchId;
    public string Name { get; }
    public List<Team> Teams { get; }
    public List<LeagueRound> Rounds { get; }
    public LeagueRound CurrentRound => Rounds.ElementAtOrDefault(roundIndex);
    public Dictionary<Team, int> Standings { get; }

    private int roundIndex;

    public League(string name)
    {
        Name = name;
        Teams = new List<Team>();
        Rounds = new List<LeagueRound>();
        roundIndex = 0;
        Standings = new Dictionary<Team, int>();
    }

    public void AddTeams(List<Team> teams)
    {
        Teams.AddRange(teams);
        foreach (var team in teams)
        {
            Standings[team] = 0;
        }
    }

    public void RemoveTeams(List<Team> teams)
    {
        foreach (var team in teams)
        {
            Teams.Remove(team);
            Standings.Remove(team);
        }
    }

    public void AddRound(LeagueRound round)
    {
        Rounds.Add(round);
    }

    public int GetNextMatchId()
    {
        return MatchId++;
    }

    public void AdvanceRound()
    {
        roundIndex++;
    }

    public bool IsOver()
    {
        return roundIndex >= Rounds.Count;
    }
}