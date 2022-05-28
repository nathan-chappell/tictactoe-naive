public interface ITicTacToeStrategy
{
    public (int Row, int Column) GetNextPlay(TicTacToeState state, PositionState name);
}