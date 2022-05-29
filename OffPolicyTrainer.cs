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
                PositionState.None => -2,
                PositionState.Player => 2,
                PositionState.Opponent => -2,
                _ => throw new NotImplementedException()
            };
            foreach (var state in states)
            {
                valueFunction.IncValue(state, reward);
            }
        }
    }

    public void DualTrain(
        EpsilonGreedyStrategy player,
        EpsilonGreedyStrategy opponent,
        int iterations)
    {
        for (int iteration = 0; iteration < iterations; ++iteration)
        {
            var firstPlayer = random.NextDouble() > .5 ? PositionState.Player : PositionState.Opponent;
            var (states, winner) = RunGame(player, opponent, firstPlayer);
            var playerReward = winner switch
            {
                PositionState.None => -1,
                PositionState.Player => 2,
                PositionState.Opponent => -2,
                _ => throw new NotImplementedException()
            };
            var opponentReward = winner switch
            {
                PositionState.None => 0,
                PositionState.Player => -2,
                PositionState.Opponent => 2,
                _ => throw new NotImplementedException()
            };
            foreach (var state in states)
            {
                player.ValueFunction.IncValue(state, playerReward);
                opponent.ValueFunction.IncValue(state, opponentReward);
            }
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
            var firstPlayer = random.NextDouble() > .5 ? PositionState.Player : PositionState.Opponent;
            var (states, winner) = RunGame(player, opponent, firstPlayer);
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
        int nonZeroStateValuesCount = -1;
        if (player is EpsilonGreedyStrategy playerGreedyStrategy && playerGreedyStrategy.ValueFunction is ValueFunction vf)
        {
            nonZeroStateValuesCount = 0;
            for (int i = 0; i < vf.Values.Length; ++i)
            {
                if (vf.Values[i] != 0)
                    ++nonZeroStateValuesCount;
            }
        }
        return new(
            Iterations: iterations,
            Draws: draws,
            Losses: losses,
            Wins: wins,
            NonZeroStateValuesCount: nonZeroStateValuesCount
        );
    }

    private (List<TicTacToeState> States, PositionState Winner) RunGame(
        ITicTacToeStrategy player,
        ITicTacToeStrategy opponent,
        PositionState firstPlayer)
    {
        var states = new List<TicTacToeState>() { new TicTacToeState(0) };
        PositionState currentPlayer = firstPlayer;
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