using System;
using NeuralNetwork.Core.Model;

namespace NeuralNetwork.Core.Learning
{
    public class ProcessData
    {
        public int GenerationIndex;
        public double BestScore;
        public double MedianScore;
        public double AverageScore;
        public double WorstScore;
        public DateTime Timestamp;
        public NetworkBase<double> BestSpecimen;
    }
}