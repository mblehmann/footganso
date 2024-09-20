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

    public IEnumerable<Team> ShiftRight(List<Team> teams)
    {
        List<Team> shiftRightTeams = new()
        {
            teams.First(),
            teams.Last()
        };
        for (int index = 1; index < teams.Count - 1; index++)
        {
            shiftRightTeams.Add(teams[index - 1]);
        }

        return shiftRightTeams;
    }
}