using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Learning.Enums;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;

namespace NeuralNetwork.Interface.Examples
{
    public class Perceptron
    {
        private LearningProcess process;
        Random rand = new Random();
        int populationCount = 20;


        public Perceptron()
        {
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfig();
            config.RandOptions = new RandomizerOptions(-1, 1, 0.3);
            config.PercentToSelect = 0.5;
            config.MutationChance = 0.2;
            config.SetParentChoosingMethod(ParentChoosingMethod.ScoreCubic);
            process = new LearningProcess(populationCount, config, new List<int> { 3, 2 }, new List<Type>() {typeof(IdentityNeuron)});
        }

        public void Run()
        {
            int counter = 0;
            while (true)
            {

                double[] scores = new double[populationCount];

                var testData = new[]
                {
                    new[] {0.2, -1.1, 0.9},
                    new[] {0.3, -2.1, -3},
                    new[] {1, -1, 1.0},
                    new[] {0, 0, 0.0},
                    new[] {-0.2, 1.1, -0.9},
                };

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
                    for (int i = 0; i < testData.Length; i++)
                    {
                        Console.WriteLine("----------------------------");
                        var a = spec.Calculate(testData[i]);
                        Console.WriteLine($"network: {a[0]} {a[1]}");
                        var a2 = Problem(testData[i]);
                        Console.WriteLine($"exptected: {a2[0]} {a2[1]}");

                        Console.WriteLine("Fitness " + FitnessFunction(testData[i], a));
                    }
                    Console.WriteLine("----------------------------");
                    var a1 = spec.Calculate(test);
                    Console.WriteLine($"network: {a1[0]} {a1[1]}");
                    var fit = FitnessFunction(test, a1);
                    a1 = Problem(test);
                    Console.WriteLine($"exptected: {a1[0]} {a1[1]}");
                    var neuron0 = spec.Layers[0].Neurons[0].GetWeights();
                    var neuron1 = spec.Layers[0].Neurons[1].GetWeights();
                    Console.WriteLine("Fitness " + fit);
                    Console.WriteLine("----------------------------");
                    Console.WriteLine(
                        $"({Math.Round(neuron0[0], 2)}x1 {Math.Round(neuron0[1], 2)}x2 {Math.Round(neuron0[2], 2)}x3 {Math.Round(neuron0[3],3)})({Math.Round(neuron1[0], 2)}x1 {Math.Round(neuron1[1], 2)}x2 {Math.Round(neuron1[2], 2)}x3 {Math.Round(neuron1[3], 3)})");
                    Console.WriteLine("Expected: ");
                    Console.WriteLine($"(x1 + x2 - 2x3 + 0)(x1 - x2 + x3 + 1)");
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
            return new[] {input[0] + input[1] - 2 * input[2], input[0] - input[1] + input[2] + 1};
        }

        public static double FitnessFunction(double[] inputs, double[] output)
        {
            var needed = Problem(inputs);
            double error = Math.Abs(output[0] - needed[0]) + Math.Abs(output[1] - needed[1]);
            return -error;
        }
    }
}