[Flags]
public enum PositionState
{
    None = 0,
    Player = 1,
    Opponent = 2,
    Both = 3,
}

public static class PositionStateExtensions
{
    public static string AsSingleCharacter(this PositionState positionState) => positionState switch
    {
        PositionState.None => "_",
        PositionState.Player => "x",
        PositionState.Opponent => "o",
        _ => "!"
    };

    public static PositionState OtherPlayer(this PositionState positionState) => positionState switch
    {
        PositionState.Player => PositionState.Opponent,
        PositionState.Opponent => PositionState.Player,
        _ => throw new NotImplementedException()
    };
}