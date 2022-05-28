public class ConsoleGame
{
    private static Random _random = new();
    private ITicTacToeStrategy opponentStrategy;

    public ConsoleGame(ITicTacToeStrategy opponentStrategy)
    {
        this.opponentStrategy = opponentStrategy;
    }

    public List<TicTacToeState> Start(PositionState playerName = PositionState.None)
    {
        var trace = new List<TicTacToeState>() { new(0) };
        
        if (playerName == PositionState.None)
            playerName = _random.NextDouble() > .5 ? PositionState.Player : PositionState.Opponent;
        
        var opponentName = playerName == PositionState.Player ? PositionState.Opponent : PositionState.Player;
        
        Console.WriteLine($"Starting game, you are: {playerName}");
        
        // while (true)
        for (int i = 0; i < 9; ++i)
        {
            var currentState = trace.Last();
            Console.WriteLine(currentState.GetDisplayString());
            var winner = currentState.GetWinner();
            if (winner != PositionState.None)
            {
                Console.WriteLine($"Winner: {winner}");
                break;
            }
            var nextPlayer = currentState.GetNextPlayer();
            Console.WriteLine($"Next player: {nextPlayer}");
            (int Row, int Column) nextPlay;
            if (nextPlayer == playerName)
            {
                nextPlay = GetPlayFromInput();
            }
            else
            {
                nextPlay = opponentStrategy.GetNextPlay(currentState, opponentName);
            }
            var nextState = currentState.WithPosition(nextPlay.Row, nextPlay.Column, nextPlayer);
            trace.Add(nextState);
        }
        return trace;
    }

    public (int Row, int Column) GetPlayFromInput()
    {
        while (true)
        try
        {
            Console.WriteLine("enter move: row column");
            var input = Console.ReadLine();
            var row = int.Parse(input[0].ToString());
            var column = int.Parse(input[1].ToString());
            return (row, column);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}