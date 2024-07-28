using System;
using System.Collections.Generic;
using System.Linq;

public class Shuffler : IShuffler
{
    public Random RNG;
    
    public Shuffler()
    {
        RNG = new Random();
    }

    public IEnumerable<Team> ShuffleTeams(List<Team> teams)
    {
        return teams.OrderBy(n => RNG.Next());
    }
}