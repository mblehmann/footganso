using System.Collections.Generic;
using System.Linq;

public class StubShuffler : IShuffler
{
    public StubShuffler()
    {

    }

    public IEnumerable<Team> ShuffleTeams(List<Team> teams)
    {
        return teams.OrderBy(t => t.Name).ToList();
    }
}