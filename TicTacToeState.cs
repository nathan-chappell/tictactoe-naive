
using System.Text;

public readonly struct TicTacToeState
{
    public readonly int Representation;

    public TicTacToeState(int representation = 0)
    {
        Representation = representation;
    }

    public int GetPositionOffset(int row, int column) => 2 * (row * 3 + column);
    public int GetPosition(int row, int column) => (Representation >> GetPositionOffset(row, column)) & 0x03;
    public PositionState GetPositionState(int row, int column) => GetPosition(row, column) switch
    {
        0 => PositionState.None,
        1 => PositionState.Player,
        2 => PositionState.Opponent,
        _ => throw new NotImplementedException($"Invalid value for Position: {GetPosition(row, column):X2}")
    };

    public TicTacToeState WithPosition(int row, int column, PositionState positionState)
    {
        var existingState = GetPositionState(row, column);
        if (existingState == PositionState.None)
            return new(Representation | ((int)positionState << GetPositionOffset(row, column)));
        else
            throw new ArgumentException($"{positionState} tried to set ({row},{column}), but it is already {existingState}");
    }

    public List<PositionState> GetPattern(int rowStart, int rowInc, int columnStart, int columnInc)
    {
        var result = new List<PositionState>();
        for (int i = 0; i < 3; ++i)
        {
            result.Add(GetPositionState(rowStart + i * rowInc, columnStart + i * columnInc));
        }
        return result;
    }

    public List<PositionState> GetRow(int row) => GetPattern(row, 0, 0, 1);
    public List<PositionState> GetColumn(int column) => GetPattern(0, 1, column, 0);
    public List<PositionState> GetDiagonalFromTopLeft() => GetPattern(0, 1, 0, 1);
    public List<PositionState> GetDiagonalFromTopRight() => GetPattern(0, 1, 2, -1);

    public PositionState GetNextPlayer()
    {
        int playerCount = 0;
        int opponentCount = 0;
        for (int row = 0; row < 3; ++row)
            for (int column = 0; column < 3; ++column)
                switch(GetPositionState(row, column))
                {
                    case PositionState.Player: playerCount++; break;
                    case PositionState.Opponent: opponentCount++; break;
                    default: break;
                }

        if (playerCount > opponentCount)
            return PositionState.Opponent;
        else
            return PositionState.Player;
    }

    public PositionState GetWinner()
    {
        for (int row = 0; row < 3; ++row)
        {
            var rowStates = GetRow(row);
            if (rowStates.Skip(1).All(positionState => positionState == rowStates.First()))
                return rowStates.First();
        }
        for (int column = 0; column < 3; ++column)
        {
            var columnStates = GetRow(column);
            if (columnStates.Skip(1).All(positionState => positionState == columnStates.First()))
                return columnStates.First();
        }
        {
            var diagonalStates = GetDiagonalFromTopLeft();
            if (diagonalStates.Skip(1).All(positionState => positionState == diagonalStates.First()))
                return diagonalStates.First();
        }
        {
            var diagonalStates = GetDiagonalFromTopRight();
            if (diagonalStates.Skip(1).All(positionState => positionState == diagonalStates.First()))
                return diagonalStates.First();
        }
        return PositionState.None;
    }

    public string GetDisplayString()
    {
        var sb = new StringBuilder();
        for (int row = 0; row < 3; ++row)
        {
            for (int column = 0; column < 2; ++column)
            {
                sb.Append(GetPositionState(row, column).AsSingleCharacter());
                sb.Append(" ");
            }
            sb.Append(GetPositionState(row, 2).AsSingleCharacter());
            sb.AppendLine();
        }
        return sb.ToString();
    }

}