using System.Collections.Generic;
using System.Linq;

public class League : Tournament
{
    public List<LeagueRound> Rounds { get; }
    public LeagueRound CurrentRound => Rounds.ElementAtOrDefault(roundIndex);
    public Dictionary<Team, int> Standings { get; }

    private int roundIndex;

    public League(string name) : base(name)
    {
        Rounds = new List<LeagueRound>();
        roundIndex = 0;
        Standings = new Dictionary<Team, int>();
    }

    public override void AddTeams(List<Team> teams)
    {
        base.AddTeams(teams);
        foreach (var team in teams)
        {
            Standings[team] = 0;
        }
    }

    public override void RemoveTeams(List<Team> teams)
    {
        base.RemoveTeams(teams);
        foreach (var team in teams)
        {
            Standings.Remove(team);
        }
    }

    public void AddRound(LeagueRound round)
    {
        Rounds.Add(round);
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