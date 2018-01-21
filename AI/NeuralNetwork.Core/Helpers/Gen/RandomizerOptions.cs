using System;
using System.Security.Policy;
using System.Xml.Serialization;
using Troschuetz.Random;
using Troschuetz.Random.Distributions.Continuous;
using Troschuetz.Random.Generators;
namespace NeuralNetwork.Core.Helpers.Gen
{
    [Serializable]
    public class RandomizerOptions
    {
        [XmlIgnore]
        public NormalDistribution Normal { get; set; }
        [XmlIgnore]
        public ContinuousUniformDistribution Uniform { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Mean { get; set; }
        public double Sigma { get; set; }

        public RandomizerOptions() { }
        public RandomizerOptions(double min, double max, double sigma = 0.25)
        {
            Min = min;
            Max = max;
            Mean = (Min + Max) / 2.0;
            Sigma = sigma;
            Normal = new NormalDistribution(Mean, Sigma);
            Uniform = new ContinuousUniformDistribution(Min,Max);
        }

        public void Reinitialize()
        {
            Normal = new NormalDistribution(Mean, Sigma);
            Uniform = new ContinuousUniformDistribution(Min, Max);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RandomizerOptions))
                return false;
            var tmp = (RandomizerOptions)obj;
            return obj.GetType() == GetType()
                   && Min == tmp.Min
                   && Max == tmp.Max
                   && Mean == tmp.Mean
                   && Sigma == tmp.Sigma;
        }
    }
}