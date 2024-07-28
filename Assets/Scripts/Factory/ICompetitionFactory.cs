public interface ICompetitionFactory
{
    public Cup CreateCup(string cupName);
    public Round CreateRound(CupPhase phase);
    public Match CreateMatch(int matchId, Team homeTeam, Team awayTeam);
}