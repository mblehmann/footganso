using System.Collections.Generic;
using System.Linq;

public class LeagueRound
{
    public List<Match> Matches { get; }
    public int Index { get; }

    public LeagueRound(int index)
    {
        Index = index;
        Matches = new List<Match>();
    }

    public void AddMatches(List<Match> matches)
    {
        Matches.AddRange(matches);
    }
}