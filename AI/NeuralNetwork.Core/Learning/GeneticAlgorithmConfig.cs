using System;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning.Enums;
using NeuralNetwork.Core.Learning.Factories;

namespace NeuralNetwork.Core.Learning
{
    public class GeneticAlgorithmConfig
    {
        public double PercentToSelect { get; set; }
        public Func<int, double[], int[]> ParentChances { get; set; }
        public RandomizerOptions RandOptions { get; set; }
        public double MutationChance { get; set; }

        public void SetParentChoosingMethod(ParentChoosingMethod method)
        {
            ParentChances = ParentChoosingFactory.Get(method);
        }
    }
}