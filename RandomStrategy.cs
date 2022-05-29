public class RandomStrategy : ITicTacToeStrategy
{
    Random _random = new();

    public RandomStrategy() { }

    public (int Row, int Column) GetNextMove(TicTacToeState state, PositionState name)
    {
        var availableMoves = GetAvailableMoves(state, name);
        return availableMoves[_random.Next(availableMoves.Count)];
    }

    private List<(int Row, int Column)> GetAvailableMoves(TicTacToeState state, PositionState name)
    {
        var winningMoves = TicTacToeState.GetWinningMoves(state, name);
        if (winningMoves.Any())
            return winningMoves;
        
        var losingMoves = TicTacToeState.GetWinningMoves(state, name.OtherPlayer());
        if (losingMoves.Any())
            return losingMoves;

        var result = new List<(int Row, int Column)>();
        for (int row = 0; row < 3; ++row)
            for (int column = 0; column < 3; ++column)
                if (state.GetPositionState(row,column) == PositionState.None)
                    result.Add((row,column));

        return result;
    }
}