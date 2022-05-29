
using System.Text;

public readonly struct TicTacToeState
{
    public static readonly int REPRESENTATION_SIZE = (int)Math.Pow(4, 9);
    public static int TotalNumberOfStates { get; }

    static TicTacToeState()
    {
        TotalNumberOfStates = GetAllStates().Count();
    }

    public readonly int Representation;

    public TicTacToeState(int representation = 0)
    {
        Representation = representation;
    }

    public bool IsValid
    {
        get
        {
            var (playerCount, opponentCount, _) = GetPositionCounts();
            var diff = Math.Abs(playerCount - opponentCount);
            if (diff > 1)
                return false;
            return GetWinner() switch
            {
                PositionState.Player => playerCount >= opponentCount,
                PositionState.Opponent => opponentCount >= playerCount,
                PositionState.Both => false,
                PositionState.None => true,
                _ => throw new NotImplementedException(),
            };
        }
    }

    public int GetPositionOffset(int row, int column) => 2 * (row * 3 + column);
    public int GetPosition(int row, int column) => (Representation >> GetPositionOffset(row, column)) & 0x03;
    public PositionState GetPositionState(int row, int column) => GetPosition(row, column) switch
    {
        0 => PositionState.None,
        1 => PositionState.Player,
        2 => PositionState.Opponent,
        3 => PositionState.Both,
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

    public IEnumerable<(int Row, int Column)> GetAvailableMoves()
    {
        for (int row = 0; row < 3; ++row)
            for (int column = 0; column < 3; ++column)
                if (GetPositionState(row,column) == PositionState.None)
                    yield return (row, column);
    }

    public static List<(int Row, int Column)> GetWinningMoves(TicTacToeState state, PositionState name)
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

    public static int GetWinningIndex(List<PositionState> positionStates, PositionState name)
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

    public (int PlayerCount, int OpponentCount, int NoneCount) GetPositionCounts()
    {
        int playerCount = 0;
        int opponentCount = 0;
        int noneCount = 0;
        for (int row = 0; row < 3; ++row)
            for (int column = 0; column < 3; ++column)
                switch (GetPositionState(row, column))
                {
                    case PositionState.Player: ++playerCount; break;
                    case PositionState.Opponent: ++opponentCount; break;
                    case PositionState.None: ++noneCount; break;
                    default: break;
                }
        return (playerCount, opponentCount, noneCount);
    }

    public PositionState GetNextPlayer()
    {
        var (playerCount, opponentCount, _) = GetPositionCounts();
        if (playerCount > opponentCount)
            return PositionState.Opponent;
        else
            return PositionState.Player;
    }

    public PositionState GetWinner()
    {
        var winner = PositionState.None;
        for (int row = 0; row < 3; ++row)
        {
            var rowStates = GetRow(row);
            if (rowStates.Skip(1).All(positionState => positionState == rowStates.First()))
                winner |= rowStates.First();
        }
        for (int column = 0; column < 3; ++column)
        {
            var columnStates = GetColumn(column);
            if (columnStates.Skip(1).All(positionState => positionState == columnStates.First()))
                winner |= columnStates.First();
        }
        {
            var diagonalStates = GetDiagonalFromTopLeft();
            if (diagonalStates.Skip(1).All(positionState => positionState == diagonalStates.First()))
                winner |= diagonalStates.First();
        }
        {
            var diagonalStates = GetDiagonalFromTopRight();
            if (diagonalStates.Skip(1).All(positionState => positionState == diagonalStates.First()))
                winner |= diagonalStates.First();
        }
        return winner;
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

    public static IEnumerable<TicTacToeState> GetAllStates()
    {
        var stateDigits = new int[9];
        Array.Fill(stateDigits, 0);

        void inc()
        {
            int carry = 1;
            for (int i = 8; i >= 0; --i)
            {
                switch (stateDigits[i])
                {
                    case 0: stateDigits[i] = carry; carry = 0; break;
                    case 1: stateDigits[i] = 1 + carry; carry = 0; break;
                    case 2: stateDigits[i] = (2 + carry) % 3; carry = (2 + carry) / 3; break;
                }
            }
        }

        bool done() => stateDigits.All(x => x == 2);

        TicTacToeState getState()
        {
            int representation = 0;
            for (int row = 0; row < 3; ++row)
                for (int column = 0; column < 3; ++column)
                    representation |= stateDigits[row * 3 + column] << 2 * (row * 3 + column);

            return new(representation);
        }

        while (!done())
        {
            var nextState = getState();
            if (nextState.IsValid)
                yield return nextState;
            inc();
        }
    }
}