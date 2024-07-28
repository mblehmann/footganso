using System;

public class Match
{
    public int Id { get; }
    public Team HomeTeam { get; }
    public Team AwayTeam { get; }
    public MatchResult MatchResult { get; private set; }
    public Team Winner { get; private set; }
    public Team Loser { get; private set; }

    public Match(int matchId, Team homeTeam, Team awayTeam)
    {
        Id = matchId;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        MatchResult = null;
        Winner = null;
        Loser = null;
    }

    public void SetResult(MatchResult result)
    {
        if (MatchResult != null) return;

        MatchResult = result;
        Winner = MatchResult.Outcome == CupOutcome.HomeAdvance ? HomeTeam : AwayTeam;
        Loser = MatchResult.Outcome == CupOutcome.HomeAdvance ? AwayTeam : HomeTeam;
    }

   public override bool Equals(object obj)
    {
        if (obj is Match otherMatch)
        {
            return Id == otherMatch.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}