// var game = new ConsoleGame(opponentStrategy);
// game.Start();

// var output = System.Console.Out;
var timestr = DateTime.UtcNow;
using var output = new StreamWriter($"/home/nathan/programming/cs/tictactoe/data/training_output_{timestr.ToString("yyyyMMdd_HHmmss")}.txt");

var playerGreedyStrategy = new EpsilonGreedyStrategy(new ValueFunction(PositionState.Player), .05);

var playerStrategies = new ITicTacToeStrategy[]
{
    // new RandomStrategy(),
    playerGreedyStrategy,
};

var opponentGreedy = new EpsilonGreedyStrategy(new ValueFunction(PositionState.Player), .2);
Util.Train(opponentGreedy, new RandomStrategy(), output: output);

var opponentStrategies = new ITicTacToeStrategy[]
{
    new RandomStrategy(),
    opponentGreedy,
};

foreach (var playerStrategy in playerStrategies)
    foreach (var opponentStrategy in opponentStrategies)
    {
        output.WriteLine($"Player: {playerStrategy.GetType().Name} vs {opponentStrategy.GetType().Name}");
        Util.Train(playerStrategy, opponentStrategy, output: output);
    }

output.WriteLine("\n---Dual Training---");

Util.DualTrain(playerGreedyStrategy, opponentGreedy, output: output);

foreach (var opponentStrategy in opponentStrategies)
{
    output.WriteLine($"Player: {playerGreedyStrategy.GetType().Name} vs {opponentStrategy.GetType().Name}");
    Util.Train(playerGreedyStrategy, opponentStrategy, output: output);
}

// if (args.Length > 1 && args[1] == "-play")
{
    // var game = new ConsoleGame(opponentStrategy);
    // var opponentStrategy = new EpsilonGreedyStrategy(playerGreedyStrategy.ValueFunction.SwapRoles(), 0);
    // var game = new ConsoleGame(opponentStrategy);
    var game = new ConsoleGame(playerGreedyStrategy);
    game.Start(PositionState.Opponent);
}


public static class Util
{
    const int defaultEvaluationIterations = 2000;
    const int defaultTrainingIterations = 2000;
    const int defaultEpochs = 5;

    public static void Train(
        ITicTacToeStrategy playerStrategy,
        ITicTacToeStrategy? opponentStrategy = null,
        int evaluationIterations = defaultEvaluationIterations,
        int trainingIterations = defaultTrainingIterations,
        int epochs = defaultEpochs,
        StreamWriter output = null
        )
    {
        if (opponentStrategy == null)
            opponentStrategy = new RandomStrategy();
        
        if (output == null)
            output = (StreamWriter)System.Console.Out;
        
        var trainer = new OffPolicyTrainer();

        if (evaluationIterations > 0)
            output.WriteLine(EvaluationResult.Header);

        for (var epoch = 0; epoch < epochs; ++epoch)
        {
            if (evaluationIterations > 0)
            {
                var evaluationResult = trainer.Evaluate(playerStrategy, opponentStrategy, evaluationIterations);
                output.WriteLine(evaluationResult.GetDisplayRow());
            }
            if (playerStrategy is EpsilonGreedyStrategy playerGreedyStrategy)
                trainer.Train(playerStrategy, opponentStrategy, playerGreedyStrategy.ValueFunction, trainingIterations);
        }
    }

    public static void DualTrain(
        EpsilonGreedyStrategy playerStrategy,
        EpsilonGreedyStrategy opponentStrategy,
        int evaluationIterations = defaultEvaluationIterations,
        int trainingIterations = defaultTrainingIterations,
        int epochs = defaultEpochs * 5,
        StreamWriter output = null
        )
    {
        var trainer = new OffPolicyTrainer();

        if (evaluationIterations > 0)
            output.WriteLine(EvaluationResult.Header);
        
        if (output == null)
            output = (StreamWriter)System.Console.Out;

        for (var epoch = 0; epoch < epochs; ++epoch)
        {
            if (evaluationIterations > 0)
            {
                var evaluationResult = trainer.Evaluate(playerStrategy, opponentStrategy, evaluationIterations);
                output.WriteLine(evaluationResult.GetDisplayRow());
            }
            if (playerStrategy is EpsilonGreedyStrategy playerGreedyStrategy)
                trainer.DualTrain(playerStrategy, opponentStrategy, trainingIterations);
        }
    }
}
