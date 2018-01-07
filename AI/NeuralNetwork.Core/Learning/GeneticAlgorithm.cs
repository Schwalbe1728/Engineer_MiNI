using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Model;
using Troschuetz.Random;

namespace NeuralNetwork.Core.Learning
{
    public class GeneticAlgorithm
    {
        public List<KeyValuePair<double, NetworkBase<double>>> Population;
        public int PopulationCount;
        public GeneticAlgorithmConfig Config;
        public TRandom Random;

        public GeneticAlgorithm()
        {
            Random = new TRandom();
            Config = new GeneticAlgorithmConfig();
            Config.RandOptions = new RandomizerOptions(-1, 1);
        }

        public void Prepare(NetworkBase<double>[] population, double[] scores)
        {
            PopulationCount = population.Length;
            Population = new List<KeyValuePair<double, NetworkBase<double>>>();
            for (int i = 0; i < population.Length; i++)
                Population.Add(new KeyValuePair<double, NetworkBase<double>>(scores[i], population[i]));

            Population = Population.OrderByDescending(x => x.Key).ToList();
        }

        public ProcessData MakeGenerationSummary(int generationIndex)
        {
            double median;
            if (Population.Count % 2 == 0)
            {
                var index = (Population.Count) / 2;
                median = (Population[index].Key + Population[index - 1].Key) / 2.0;
            }
            else
                median = Population[(Population.Count - 1) / 2].Key;

            ProcessData data = new ProcessData()
            {
                GenerationIndex = generationIndex,
                BestScore = Population[0].Key,
                BestSpecimen = Population[0].Value.Copy(),
                WorstScore = Population.Last().Key,
                AverageScore = Population.Average(x => x.Key),
                MedianScore = median,
                Timestamp = DateTime.Now
            };

            return data;

        }

        public NetworkBase<double>[] GetNewGeneration()
        {
            Selection();
            Crosbreeding();
            Mutation();
            return Population.Select(x => x.Value).ToArray();
        }

        public void Selection()
        {
            var count = (int) (Config.PercentToSelect * PopulationCount);
            Population = Population.Take(count).ToList();
        }

        public void Crosbreeding()
        {
            var weights = Config.ParentChances(Population.Count, Population.Select(x=>x.Key).ToArray());
            var total = weights.Sum();

            while (Population.Count < PopulationCount)
            {
                var indexes = ChooseParents(weights, total);
                NetworkBase<double> female = Population[indexes[0]].Value;
                NetworkBase<double> male = Population[indexes[1]].Value;

                NetworkBase<double> child1 = female.Copy();
                NetworkBase<double> child2 = male.Copy();

                RandomNeuronSwap(child1, child2);

                Population.Add(new KeyValuePair<double, NetworkBase<double>>(0, child1));
                if (Population.Count != PopulationCount)
                    Population.Add(new KeyValuePair<double, NetworkBase<double>>(0, child2));
            }
        }

        public void RandomNeuronSwap(NetworkBase<double> child1, NetworkBase<double> child2)
        {
            for (int i = 0; i < child1.LayerCount; i++)
            {
                for (int k = 0; k < child1.Layers[i].NeuronCount; k++)
                {
                    if (Random.NextBoolean())
                    {
                        var tmp = child1.Layers[i].Neurons[k];
                        child1.Layers[i].Neurons[k] = child2.Layers[i].Neurons[k];
                        child2.Layers[i].Neurons[k] = tmp;
                    }
                }
            }
        }

        private int[] ChooseParents(int[] weights, int sum)
        {
            var result = new int[2];
            int[] value = {0, 0};
            while (value[0] == value[1]) //prevent parent from reproducing with itself
                value = Random.Integers(sum).Take(2).ToArray();
            bool first = true, second = true;

            for (int i = 0; i < weights.Length; i++)
            {
                if (first && weights[i] > value[0])
                {
                    result[0] = i;
                    first = false;
                }
                if (second && weights[i] > value[1])
                {
                    result[1] = i;
                    second = false;
                }

                if (!second && !first)
                    break;
            }

            return result;
        }

        public void Mutation()
        {
            bool first = true;
            foreach (var specimen in Population)
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                var net = specimen.Value;
                foreach (var layer in net.Layers)
                {
                    foreach (var neuron in layer.Neurons)
                    {
                        var weights = neuron.GetWeights();
                        for (var i = 0; i < weights.Length; i++)
                        {
                            if (Random.NextDouble() < Config.MutationChance)
                            {
                                weights[i] += Config.RandOptions.Normal.NextDouble();
                            }
                        }
                    }
                }
            }
        }
    }
}