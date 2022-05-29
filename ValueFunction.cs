public class ValueFunction : IValueFunction
{
    public int[] Values { get; private set; }
    PositionState Name { get; }

    public ValueFunction(PositionState name, int[] values)
    {
        Name = name;
        Values = values;
    }

    public ValueFunction(PositionState name) : this (name, new int[TicTacToeState.REPRESENTATION_SIZE]) {}

    public int GetValue(TicTacToeState state) => Values[state.Representation];
    public int SetValue(TicTacToeState state, int value) => Values[state.Representation] = value;
    public int IncValue(TicTacToeState state, int increment) => Values[state.Representation] += increment;

    public IValueFunction SwapRoles()
    {
        var newValues = new int[Values.Length];
        for (int i = 0; i < Values.Length; ++i)
        {
            var state = new TicTacToeState(i);
            if (!state.IsValid)
                continue;
            var value = Values[i];
            var newIndex = i;
            for (int j = 0; j < 9; ++j)
            {
                newIndex ^= (0x03 << 2*j);
            }
            newValues[newIndex] = value;
        }
        var newName = Name == PositionState.Player ? PositionState.Opponent : PositionState.Player;
        return new ValueFunction(newName, newValues);
    }
}