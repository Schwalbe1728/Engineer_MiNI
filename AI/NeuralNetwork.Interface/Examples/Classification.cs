using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Learning.Enums;
using NeuralNetwork.Core.Model.Neurons;

namespace NeuralNetwork.Interface.Examples
{
    public class Classification
    {
        private LearningProcess process;
        Random rand = new Random();
        int populationCount = 200;


        public Classification()
        {
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfig();
            config.RandOptions = new RandomizerOptions(-1, 1, 0.2);
            config.PercentToSelect = 0.5;
            config.MutationChance = 0.2;
            config.SetParentChoosingMethod(ParentChoosingMethod.PositionLinear);
            process = new LearningProcess(populationCount, config, new List<int> { 2, 4, 2 }, new List<Type>() { typeof(TanHNeuron),typeof(StepNeuron) });
        }
        public void Run()
        {
            int counter = 0;
            while (true)
            {

                double[] scores = new double[populationCount];

                int dataCount = 50;
                var testData = new double[dataCount][];
                for (int i = 0; i < dataCount; i++)
                {
                    testData[i] = new[]
                    {
                        rand.NextDouble() * 2 - 1,
                        rand.NextDouble() * 2 - 1
                    };
                }

                double[,] scores2 = new double[populationCount, testData.Length];
                for (int k = 0; k < testData.Length; k++)
                {

                    for (int i = 0; i < populationCount; i++)
                    {
                        var result = process.Population[i].Calculate(testData[k]);
                        var a = FitnessFunction(testData[k], result);
                        scores[i] += a;
                        scores2[i, k] = a;
                    }
                }

                process.Learn(scores);
                var data = process.HistoricalData.Last();
                Console.WriteLine(
                    $"Gen: {data.GenerationIndex} Max: {data.BestScore} Min: {data.WorstScore} Avg: {data.AverageScore} Med: {data.MedianScore}");

                //Thread.Sleep(1000);

                if (counter == 50)
                {
                    var test = new[]
                    {
                        rand.NextDouble() * 2 - 1,
                        rand.NextDouble() * 2 - 1,
                        rand.NextDouble() * 2 - 1
                    };

                    var spec = process.HistoricalData.Last().BestSpecimen;
                    
                    Console.WriteLine("----------------------------");
                    var a1 = spec.Calculate(test);
                    Console.WriteLine($"network: {a1[0]} {a1[1]}");
                    a1 = Problem(test);
                    Console.WriteLine($"exptected: {a1[0]} {a1[1]}");
                    Console.WriteLine("----------------------------");

                    Console.ReadKey();
                    counter = 0;
                }

                counter++;
            }
        }
        public static int[] Chances(int n)
        {
            int[] result = new int[n];

            for (int i = 0; i < n; i++)
            {
                result[i] = n - i;
            }
            return result;
        }
        public static double[] Problem(double[] input)
        {
            if (input[0] >= 0)
            {
                if (input[1] >= 0)
                    return new[] {1.0, 1.0};
                else
                    return new[] {1.0, 0};
            }
            if (input[1] >= 0)
                return new[] { 0, 1.0 };
            else
                return new[] { 0.0, 0 };
        }

        public static double FitnessFunction(double[] inputs, double[] output)
        {
            var needed = Problem(inputs);
            double error = Math.Abs(output[0] - needed[0]) + Math.Abs(output[1] - needed[1]);
            return -error;
        }
    }
}