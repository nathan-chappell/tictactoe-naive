public class ValueFunction : IValueFunction
{
    public int[] Values { get; private set; }
    PositionState Name { get; }

    public ValueFunction(PositionState name)
    {
        Name = name;
        Values = new int[TicTacToeState.REPRESENTATION_SIZE];

        // foreach (var state in TicTacToeState.GetAllStates().Where(state => state.IsTerminal && state.IsValid))
        // {
        //     switch (state.GetWinner())
        //     {
        //         case PositionState.None: values[state.Representation] = -1; break;
        //         case PositionState.Player: values[state.Representation] = name == PositionState.Player ? 2 : -2; break;
        //         case PositionState.Opponent: values[state.Representation] = name == PositionState.Opponent ? 2 : -2; break;
        //     }
        // }
    }

    public int GetValue(TicTacToeState state) => Values[state.Representation];
    public int SetValue(TicTacToeState state, int value) => Values[state.Representation] = value;
    public int IncValue(TicTacToeState state, int increment) => Values[state.Representation] += increment;
}