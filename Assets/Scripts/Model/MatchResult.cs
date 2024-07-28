public class MatchResult
{
    public int MatchId { get; }
    public int HomeTeamGoals { get; }
    public int AwayTeamGoals { get; }
    public CupOutcome Outcome { get; private set; }
    
    public MatchResult(int matchId, int homeTeamGoals, int awayTeamGoals)
    {
        MatchId = matchId;
        HomeTeamGoals = homeTeamGoals;
        AwayTeamGoals = awayTeamGoals;
        SetMatchOutcome();
    }

    private void SetMatchOutcome()
    {
        Outcome = HomeTeamGoals > AwayTeamGoals ? CupOutcome.HomeAdvance : CupOutcome.AwayAdvance;
    }
}