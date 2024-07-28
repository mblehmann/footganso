using System.Collections.Generic;
using System.Linq;

public class Cup
{
    private int MatchId;
    public string Name { get; }
    public List<Team> Teams { get; }
    public Round CurrentRound => Rounds.LastOrDefault();
    private readonly List<Round> Rounds;

    public Cup(string name)
    {
        Name = name;
        Teams = new List<Team>();
        Rounds = new List<Round>();
    }

    public void AddTeams(List<Team> teams)
    {
        Teams.AddRange(teams);
    }

    public void RemoveTeams(List<Team> teams)
    {
        foreach (var team in teams)
        {
            Teams.Remove(team);
        }
    }

    public void AddRound(Round round)
    {
        Rounds.Add(round);
    }

    public int GetNextMatchId()
    {
        return MatchId++;
    }
}