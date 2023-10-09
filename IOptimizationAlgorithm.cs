public interface IOptimizationAlgorithm
{
    string HarrisHawksOptimization { get; set; }

    double Solve();

    double[] XBest { get; set; }

    double FBest { get; set; }

    int NumberOfEvaluationFitnessFunction { get; set; }
}
