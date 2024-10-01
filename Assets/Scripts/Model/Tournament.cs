using System.Collections.Generic;

public abstract class Tournament
{
    protected int MatchId;
    public string Name { get; }
    public List<Team> Teams { get; }

    protected Tournament(string name)
    {
        MatchId = 0;
        Name = name;
        Teams = new List<Team>();
    }

    public virtual void AddTeams(List<Team> teams)
    {
        Teams.AddRange(teams);
    }

    public virtual void RemoveTeams(List<Team> teams)
    {
        foreach (var team in teams)
        {
            Teams.Remove(team);
        }
    }

    public int GetNextMatchId()
    {
        return MatchId++;
    }
}