using System;
using NeuralNetwork.Core.Helpers;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model
{
    [Serializable]
    public class IdentityNeuron : INeuron<double>
    {
        private int _inputCount;

        public bool HasConstant;
        public double[] Weights { get; set; }

        public IdentityNeuron(int inputCount, bool hasConstant = true)
        {
            _inputCount = inputCount;
            HasConstant = hasConstant;

            if(HasConstant)
                Weights = new double[_inputCount + 1];
            else
                Weights = new double[_inputCount];
        }

        public double Process(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < _inputCount; i++)
            {
                sum += input[i] * Weights[i];
            }
            if (HasConstant)
                sum += Weights[_inputCount]; //Constant Neuron
            return ActivationFunctions.Identity(sum);
        }

        public double[] GetWeights()
        {
            return Weights;
        }

        public void SetWeights(double[] weights)
        {
            Weights = weights;
            if (HasConstant)
                _inputCount = Weights.Length - 1;
            else
                _inputCount = Weights.Length;
        }

        public int GetCount()
        {
            return Weights.Length;
        }
    }
}