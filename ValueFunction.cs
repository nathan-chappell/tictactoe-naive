public class ValueFunction
{
    public int[] values;
    PositionState name;

    public ValueFunction(PositionState name)
    {
        this.name = name;
        values = new int[TicTacToeState.REPRESENTATION_SIZE];

        foreach (var state in TicTacToeState.GetAllStates().Where(state => state.IsTerminal && state.IsValid))
        {
            switch (state.GetWinner())
            {
                case PositionState.None: values[state.Representation] = -1; break;
                case PositionState.Player: values[state.Representation] = name == PositionState.Player ? 2 : -2; break;
                case PositionState.Opponent: values[state.Representation] = name == PositionState.Opponent ? 2 : -2; break;
            }
        }
    }
}