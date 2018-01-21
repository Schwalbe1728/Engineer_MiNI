using System;
using System.Xml.Serialization;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning.Enums;
using NeuralNetwork.Core.Learning.Factories;

namespace NeuralNetwork.Core.Learning
{
    [Serializable]
    public class GeneticAlgorithmConfig
    {
        public double PercentToSelect { get; set; }
        [XmlIgnore]
        public Func<int, double[], int[]> ParentChances { get; set; }
        public RandomizerOptions RandOptions { get; set; }
        public double MutationChance { get; set; }
        public ParentChoosingMethod ParentMethod { get; set; }

        public void SetParentChoosingMethod(ParentChoosingMethod method)
        {
            ParentMethod = method;
            Reinitialize();
        }

        public void Reinitialize()
        {
            ParentChances = ParentChoosingFactory.Get(ParentMethod);
            RandOptions.Reinitialize();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GeneticAlgorithmConfig))
                return false;
            var tmp = (GeneticAlgorithmConfig)obj;
            return obj.GetType() == GetType()
                   && PercentToSelect == tmp.PercentToSelect
                   && MutationChance == tmp.MutationChance
                   && ParentMethod == tmp.ParentMethod
                   && RandOptions.Equals(tmp.RandOptions);
        }
    }
}