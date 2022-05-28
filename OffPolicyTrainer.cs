public class OffPolicyTrainer
{
    private Random random = new();

    public void Train(
        ITicTacToeStrategy player,
        ITicTacToeStrategy opponent,
        IValueFunction valueFunction,
        int iterations)
    {
        for (int iteration = 0; iteration < iterations; ++iteration)
        {
            var firstPlayer = random.NextDouble() > .5 ? PositionState.Player : PositionState.Opponent;
            var (states, winner) = RunGame(player, opponent, firstPlayer);
            var reward = winner switch
            {
                PositionState.None => -1,
                PositionState.Player => 2,
                PositionState.Opponent => -2,
                _ => throw new NotImplementedException()
            };
            valueFunction.IncValue(states.Last(), reward);
        }
    }

    public EvaluationResult Evaluate(
        ITicTacToeStrategy player,
        ITicTacToeStrategy opponent,
        int iterations)
    {
        int wins = 0;
        int losses = 0;
        int draws = 0;
        for (int iteration = 0; iteration < iterations; ++iteration)
        {
            // var firstPlayer = random.NextDouble() > .5 ? PositionState.Player : PositionState.Opponent;
            PositionState winner;
            if (random.NextDouble() > .5)
            {
                var (states, _winner) = RunGame(player, opponent);
                winner = _winner;
            }
            else
            {
                var (states, _winner) = RunGame(player, opponent);
                winner = _winner switch
                {
                    PositionState.None => PositionState.None,
                    PositionState.Opponent => PositionState.Player,
                    PositionState.Player => PositionState.Opponent,
                    _ => throw new NotImplementedException(),
                };
            }
            switch (winner)
            {
                case PositionState.None: draws += 1; break;
                case PositionState.Player: wins += 1; break;
                case PositionState.Opponent: losses += 1; break;
                default:
                    break;
            }
            // Console.WriteLine(states.Last().GetDisplayString());
        }
        return new(
            Iterations: iterations,
            Draws: draws,
            Losses: losses,
            Wins: wins
        );
    }

    private (List<TicTacToeState> States, PositionState Winner) RunRandomGame(
        ITicTacToeStrategy player,
        ITicTacToeStrategy opponent)
    {
        if (random.NextDouble() > .5)
        {
            var (states, _winner) = RunGame(player, opponent);
            winner = _winner;
        }
        else
        {
            var (states, _winner) = RunGame(player, opponent);
            winner = _winner switch
            {
                PositionState.None => PositionState.None,
                PositionState.Opponent => PositionState.Player,
                PositionState.Player => PositionState.Opponent,
                _ => throw new NotImplementedException(),
            };
        }
    }

    private (List<TicTacToeState> States, PositionState Winner) RunGame(
        ITicTacToeStrategy player,
        ITicTacToeStrategy opponent)
    {
        var states = new List<TicTacToeState>() { new TicTacToeState(0) };
        PositionState currentPlayer = PositionState.Player;
        PositionState winner = PositionState.None;
        while (states.Count < 9)
        {
            var state = states.Last();
            var nextMove = currentPlayer == PositionState.Player
                ? player.GetNextMove(state, currentPlayer)
                : opponent.GetNextMove(state, currentPlayer);
            var nextState = state.WithPosition(nextMove.Row, nextMove.Column, currentPlayer);
            states.Add(nextState);
            winner = nextState.GetWinner();
            if (winner != PositionState.None)
                break;
            currentPlayer = currentPlayer == PositionState.Player
                ? PositionState.Opponent
                : PositionState.Player;
        }
        return (states, winner);
    }
}