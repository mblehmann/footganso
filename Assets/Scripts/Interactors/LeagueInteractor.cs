using System.Collections.Generic;
using System.Linq;

public class LeagueInteractor
{
    private readonly IShuffler Shuffler;
    private readonly ILeaguePresenter LeaguePresenter;

    public LeagueInteractor(IShuffler shuffler, ILeaguePresenter leaguePresenter)
    {
        Shuffler = shuffler;
        LeaguePresenter = leaguePresenter;
    }

    public League CreateLeague(string leagueName)
    {
        League league = new League(leagueName);
        LeaguePresenter?.DisplayLeague(league);
        return league;
    }

    public void AddTeams(League league, List<Team> teams)
    {
        league.AddTeams(teams);
    }

    public void RemoveTeams(League league, List<Team> teams)
    {
        league.RemoveTeams(teams);
    }

    public void DrawSchedule(League league)
    {
        var shuffledTeams = Shuffler.ShuffleTeams(league.Teams).ToList();

        int numberOfRounds = 2 * (league.Teams.Count - 1);
        for (int roundIndex = 0; roundIndex < numberOfRounds; roundIndex++)
        {
            DrawRound(league, roundIndex, shuffledTeams);
            shuffledTeams = ShiftRight(shuffledTeams.ToList(), (shuffledTeams.Count() - 1) / 2);
        }
        LeaguePresenter?.DisplayRound(league.CurrentRound);
        LeaguePresenter?.DisplayStandings(league.Standings);
    }

    public void UpdateScores(League league, List<MatchResult> matchResults)
    {
        foreach (var result in matchResults)
        {
            var match = league.CurrentRound.Matches.SingleOrDefault(x => x.Id == result.MatchId);
            if (match != null)
            {
                match.SetResult(result);
                if (match.MatchResult.HomeTeamGoals > match.MatchResult.AwayTeamGoals)
                {
                    league.Standings[match.HomeTeam] += 3;
                }
                else if (match.MatchResult.HomeTeamGoals < match.MatchResult.AwayTeamGoals)
                {
                    league.Standings[match.AwayTeam] += 3;
                }
                else
                {
                    league.Standings[match.HomeTeam]++;
                    league.Standings[match.AwayTeam]++;
                }
            }
        }
        LeaguePresenter?.DisplayRound(league.CurrentRound);
    }

    public void FinishRound(League league)
    {
        LeaguePresenter?.DisplayStandings(league.Standings);
        league.AdvanceRound();
        if (!IsOver(league))
        {
            LeaguePresenter?.DisplayRound(league.CurrentRound);
        }
    }

    public bool IsOver(League league)
    {
        return league.IsOver();
    }
    
    private void DrawRound(League league, int roundIndex, List<Team> teams)
    {
        LeagueRound round = CreateRound(league, roundIndex, teams);
        league.AddRound(round);
    }

    private LeagueRound CreateRound(League league, int roundIndex, List<Team> teams)
    {
        LeagueRound round = new LeagueRound(roundIndex);
        List<Match> matches = DrawMatches(league, roundIndex, teams);
        round.AddMatches(matches);
        return round;
    }

    private List<Match> DrawMatches(League league, int roundIndex, List<Team> teams)
    {
        List<Match> matches = new List<Match>();
        int lastIndex = teams.Count() - 1;
        for (int teamIndex = 0; teamIndex < teams.Count() / 2; teamIndex++)
        {
            var matchId = league.GetNextMatchId();
            var homeTeam = teams[teamIndex];
            var awayTeam = teams[lastIndex-teamIndex];
            
            if ((teamIndex == 0 && roundIndex % 2 == 0) || (teamIndex != 0 && roundIndex >= teams.Count() - 1))
            {
                (awayTeam, homeTeam) = (homeTeam, awayTeam);
            }

            Match match = new Match(matchId, homeTeam, awayTeam);
            matches.Add(match);
        }

        return matches;
    }

    private List<Team> ShiftRight(List<Team> teams, int shift)
    {
        int length = teams.Count;
        int size = length - 1;
        List<Team> shiftRightTeams = new(new Team[length]);
        shiftRightTeams[0] = teams[0];

        for (int index = 1; index < teams.Count; index++)
        {
            int position = (index + shift) % size;
            if (position == 0)
            {
                position = size;
            }
            shiftRightTeams[position] = teams[index];
        }

        return shiftRightTeams;
    }
}
