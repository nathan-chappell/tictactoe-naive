var randomStrategy = new RandomStrategy();

var game = new ConsoleGame(randomStrategy);

// game.Start();

var playerStrategy = new EpsilonGreedyStrategy(new ValueFunction(PositionState.Player), .1);
var trainer = new OffPolicyTrainer();

var evaluationIterations = 400;
var trainingIterations = 1000;
var epochs = 40;

var evaluationResults = new List<EvaluationResult>();

for (var epoch = 0; epoch < epochs; ++epoch)
{
    evaluationResults.Add(trainer.Evaluate(playerStrategy, randomStrategy, evaluationIterations));
    trainer.Train(playerStrategy, randomStrategy, playerStrategy.ValueFunction, trainingIterations);
}

evaluationResults.Add(trainer.Evaluate(playerStrategy, randomStrategy, evaluationIterations));

Console.WriteLine(EvaluationResult.Header);
foreach (var evaluationResult in evaluationResults)
{
    Console.WriteLine(evaluationResult.GetDisplayRow());
}