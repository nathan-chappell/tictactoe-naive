public class EpsilonGreedyStrategy : ITicTacToeStrategy
{
    public double Epsilon { get; }
    public IValueFunction ValueFunction { get; }
    private Random random = new();

    public EpsilonGreedyStrategy(IValueFunction valueFunction, double epsilon)
    {
        Epsilon = epsilon;
        ValueFunction = valueFunction;
    }

    public (int Row, int Column) GetNextMove(TicTacToeState state, PositionState name)
    {
        var winningMoves = TicTacToeState.GetWinningMoves(state, name);
        if (winningMoves.Any())
            return winningMoves.First();
        
        var losingMoves = TicTacToeState.GetWinningMoves(state, name.OtherPlayer());
        if (losingMoves.Any())
            return losingMoves.First();

        var nextMoves = state.GetAvailableMoves().ToList();
        if (random.NextDouble() < Epsilon)
            return nextMoves[random.Next(0, nextMoves.Count)];
        return nextMoves.OrderBy(move => ValueFunction.GetValue(state.WithPosition(move.Row, move.Column, name))).Last();
    }
}