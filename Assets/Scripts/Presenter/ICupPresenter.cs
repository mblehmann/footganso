using System.Collections.Generic;

public interface ICupPresenter
{
    public void DisplayCup(Cup cup);
    public void DisplayTeams(List<Team> teams);
    public void DisplayRound(Round round);
}