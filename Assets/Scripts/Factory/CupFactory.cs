public class CupFactory : ICompetitionFactory
{
    public Cup CreateCup(string cupName)
    {
        return new Cup(cupName);
    }

    public Round CreateRound(CupPhase phase)
    {
        return new Round(phase);
    }

    public Match CreateMatch(int matchId, Team homeTeam, Team awayTeam)
    {
        return new Match(matchId, homeTeam, awayTeam);
    }
}