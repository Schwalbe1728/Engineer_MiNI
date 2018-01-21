using System;
using NeuralNetwork.Core.Helpers;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model.Neurons
{
    [Serializable]
    public class TanHNeuron : NeuronBase<double>
    {
        public TanHNeuron() { }
        public TanHNeuron(int inputCount, bool hasConstant = true) : base(inputCount, hasConstant)
        {
        }

        public override double Process(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < InputCount; i++)
            {
                sum += input[i] * Weights[i];
            }
            if (HasConstant)
                sum += Weights[InputCount]; //Constant Neuron
            return ActivationFunctions.TanH(sum);
        }
    }
}