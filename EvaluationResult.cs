public record EvaluationResult(int Iterations, int Draws, int Losses, int Wins, int NonZeroStateValuesCount)
{
    public static string Header => "__W__ __L__ __D__ __NZ__";
    public string GetDisplayRow()
    {
        double w = Wins / (double)Iterations;
        double l = Losses / (double)Iterations;
        double d = Draws / (double)Iterations;
        double z = NonZeroStateValuesCount / (double)TicTacToeState.TotalNumberOfStates;
        return $"{w,5:f2} {l,5:f2} {d,5:f2} {z,6:f3}";
    }
}