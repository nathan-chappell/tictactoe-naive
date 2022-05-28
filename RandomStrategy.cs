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
        var winningMoves = GetWinningMoves(state, name);
        if (winningMoves.Any())
            return winningMoves;

        var result = new List<(int Row, int Column)>();
        for (int row = 0; row < 3; ++row)
            for (int column = 0; column < 3; ++column)
                if (state.GetPositionState(row,column) == PositionState.None)
                    result.Add((row,column));

        return result;
    }

    private List<(int Row, int Column)> GetWinningMoves(TicTacToeState state, PositionState name)
    {
        var result = new List<(int Row, int Column)>();
        for (int row = 0; row < 3; ++row)
        {
            var rowStates = state.GetRow(row);
            var winningIndex = GetWinningIndex(rowStates, name);
            if (winningIndex != -1)
                result.Add((row, winningIndex));
        }
        for (int column = 0; column < 3; ++column)
        {
            var columnStates = state.GetColumn(column);
            var winningIndex = GetWinningIndex(columnStates, name);
            if (winningIndex != -1)
                result.Add((winningIndex, column));
        }
        {
            var diagonalStates = state.GetDiagonalFromTopLeft();
            var winningIndex = GetWinningIndex(diagonalStates, name);
            if (winningIndex != -1)
                result.Add((winningIndex, winningIndex));
        }
        {
            var diagonalStates = state.GetDiagonalFromTopRight();
            var winningIndex = GetWinningIndex(diagonalStates, name);
            if (winningIndex != -1)
                result.Add((winningIndex, 2 - winningIndex));
        }
        return result;
    }

    private int GetWinningIndex(List<PositionState> positionStates, PositionState name)
    {
        int winningIndex = -1;
        int usCount = 0;
        int themCount = 0;
        for (int i = 0; i < 3; ++i)
        {
            if (positionStates[i] == name)
                usCount++;
            else if (positionStates[i] == PositionState.None)
                winningIndex = i;
            else
                themCount++;
        }
        if (usCount == 2 && themCount == 0)
            return winningIndex;
        else
            return -1;
    }
}