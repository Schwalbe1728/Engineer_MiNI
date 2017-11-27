using System;
using NeuralNetwork.Core.Helpers.Gen;

namespace NeuralNetwork.Core.Learning
{
    public class GeneticAlgorithmConfig
    {
        public double PercentToSelect { get; set; }
        public Func<int, int[]> ParentChances { get; set; }
        public RandomizerOptions RandOptions { get; set; }
        public double MutationChance { get; set; }
    }
}