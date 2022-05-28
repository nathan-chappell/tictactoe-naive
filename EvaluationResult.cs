public record EvaluationResult(int Iterations, int Draws, int Losses, int Wins)
{
    public static string Header => "__W__ __L__ __D__";
    public string GetDisplayRow()
    {
        double w = Wins / (double)Iterations;
        double l = Losses / (double)Iterations;
        double d = Draws / (double)Iterations;
        return $"{w,5:f2} {l,5:f2} {d,5:f2}";
    }
}