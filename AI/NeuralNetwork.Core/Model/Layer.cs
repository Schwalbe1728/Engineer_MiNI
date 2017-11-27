using System;
using System.Runtime.InteropServices;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model
{
    [Serializable]
    public class Layer<T> : LayerBase<T>
    {
        public Layer(int neuronCount, int inputCount) : base(neuronCount,inputCount)
        { }
        public Layer() { }
    }
}