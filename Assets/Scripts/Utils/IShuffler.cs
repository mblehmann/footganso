using System.Collections.Generic;

public interface IShuffler
{
    public IEnumerable<Team> ShuffleTeams(List<Team> teams);
    public IEnumerable<Team> ShiftRight(List<Team> teams);
}