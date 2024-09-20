using System.Collections.Generic;

public interface ILeaguePresenter
{
    public void DisplayLeague(League league);
    public void DisplayStandings(Dictionary<Team, int> standings);
    public void DisplayRound(LeagueRound round);
}