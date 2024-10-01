using System.Collections.Generic;
using System.Linq;

public class Round
{
    public CupPhase Phase { get; }
    public List<Match> Matches { get; }
    public int NumberOfMatches => Matches.Count;
    public List<Team> Winners => Matches.Select(team => team.Winner).ToList();
    public List<Team> Losers => Matches.Select(team => team.Loser).ToList();

    public Round(CupPhase phase)
    {
        Phase = phase;
        Matches = new List<Match>();
    }

    public void AddMatches(List<Match> matches)
    {
        Matches.AddRange(matches);
    }
}