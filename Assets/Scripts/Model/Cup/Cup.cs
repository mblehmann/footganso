using System.Collections.Generic;
using System.Linq;

public class Cup : Tournament
{
    public Round CurrentRound => Rounds.LastOrDefault();
    private readonly List<Round> Rounds;

    public Cup(string name) : base(name)
    {
        Rounds = new List<Round>();
    }

    public void AddRound(Round round)
    {
        Rounds.Add(round);
    }
}