using System;
using NeuralNetwork.Core.Model;

namespace NeuralNetwork.Core.Learning
{
    [Serializable]
    public class ProcessData
    {
        public int GenerationIndex;
        public double BestScore;
        public double MedianScore;
        public double AverageScore;
        public double WorstScore;
        public DateTime Timestamp;
        public NetworkBase<double> BestSpecimen;

        public override bool Equals(object obj)
        {
            double t = 0.00000001;
            if(!(obj is ProcessData))
                return false;
            var tmp = (ProcessData) obj;
            return GenerationIndex == tmp.GenerationIndex
                   && Math.Abs(BestScore - tmp.BestScore) < t
                   && Math.Abs(MedianScore - tmp.MedianScore) < t
                   && Math.Abs(AverageScore - tmp.AverageScore) < t
                   && Math.Abs(WorstScore - tmp.WorstScore) < t
                   && Timestamp == tmp.Timestamp
                   && BestSpecimen.Equals(tmp.BestSpecimen);
        }
    }
}