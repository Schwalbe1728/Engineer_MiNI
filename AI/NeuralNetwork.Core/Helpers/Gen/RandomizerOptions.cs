using System;
using Troschuetz.Random;
using Troschuetz.Random.Distributions.Continuous;
using Troschuetz.Random.Generators;
namespace NeuralNetwork.Core.Helpers.Gen
{
    public class RandomizerOptions
    {
        public NormalDistribution Normal { get; set; }
        public ContinuousUniformDistribution Uniform { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Mean { get; set; }
        public double Sigma { get; set; }

        public RandomizerOptions(double min, double max, double sigma = 0.25)
        {
            Min = min;
            Max = max;
            Mean = (Min + Max) / 2.0;
            Sigma = sigma;
            Normal = new NormalDistribution(Mean, Sigma);
            Uniform = new ContinuousUniformDistribution(Min,Max);
        }
    }
}