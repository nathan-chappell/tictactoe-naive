public interface ITicTacToeStrategy
{
    public (int Row, int Column) GetNextMove(TicTacToeState state, PositionState name);
}