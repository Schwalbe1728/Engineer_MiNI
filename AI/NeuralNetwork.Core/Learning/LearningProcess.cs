using System;
using System.Collections.Generic;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Model;

namespace NeuralNetwork.Core.Learning
{
    public class LearningProcess
    {
        public NetworkBase<double>[] Population { get; set; }
        public int Generation { get; set; }
        public int PopulationCount { get; set; }
        public int BestIndex { get; set; }
        public List<ProcessData> HistoricalData { get; set; }
        public GeneticAlgorithm LearningAlgorithm { get; set; }

        public LearningProcess()
        {
            LearningAlgorithm = new GeneticAlgorithm();
            Generation = 0;
            HistoricalData = new List<ProcessData>();
        }

        public LearningProcess(int populationCount, GeneticAlgorithmConfig alg) : base()
        {
            NewRandomPopulation(populationCount, new List<int> {3,5,2}
            , new List<Type> {typeof(IdentityNeuron),typeof(IdentityNeuron)} );
            Generation = 0;
            HistoricalData = new List<ProcessData>();
            LearningAlgorithm.Config = alg;
        }

        public void Learn(double[] scores)
        {
            LearningAlgorithm.Prepare(Population,scores);
            HistoricalData.Add(LearningAlgorithm.MakeGenerationSummary(Generation));
            Generation++;
            Population = LearningAlgorithm.GetNewGeneration();
        }


        public void NewRandomPopulation(int populationCount, List<int> layerCount, List<Type> neuronTypes)
        {
            Generation = 0;
            PopulationCount = populationCount;
            Population = new NetworkBase<double>[PopulationCount];
            LearningAlgorithm = new GeneticAlgorithm();


            var builder = Builder.GetBuilder(layerCount, neuronTypes);
            for (int i = 0; i < PopulationCount; i++)
                Population[i] = builder.GetEmpty().Randomize();
        }
    }
}