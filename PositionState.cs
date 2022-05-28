public enum PositionState
{
    None = 0,
    Player = 1,
    Opponent = 2,
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
}