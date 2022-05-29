public interface IValueFunction
{
    int GetValue(TicTacToeState state);
    int IncValue(TicTacToeState state, int increment);
    int SetValue(TicTacToeState state, int value);
    IValueFunction SwapRoles();
}
