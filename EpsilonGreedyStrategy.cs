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
        var nextMoves = state.GetAvailableMoves().ToList();
        if (random.NextDouble() < Epsilon)
            return nextMoves[random.Next(0, nextMoves.Count)];
        return nextMoves.OrderBy(move => ValueFunction.GetValue(state.WithPosition(move.Row,move.Column,name))).Last();
    }
}