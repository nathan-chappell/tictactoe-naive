var randomStrategy = new RandomStrategy();

var game = new ConsoleGame(randomStrategy);

// game.Start();

var vf = new ValueFunction(PositionState.Player);

Console.WriteLine($"You are: {PositionState.Player}");

foreach (var state in TicTacToeState.GetAllStates().Where(state => state.IsTerminal && state.IsValid).Take(100))
{
    Console.WriteLine($"Winner: {state.GetWinner()}");
    Console.WriteLine($"Value: {vf.values[state.Representation]}");
    Console.WriteLine(state.GetDisplayString());
}