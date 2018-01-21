using System.Collections.Generic;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;

namespace NeuralNetwork.Core.Test.Setup
{
    public static class Values

    {
        public static readonly object[] IdentityNeuronTestsC =
        {
            new object[]
            {
                3,
                new[] { 0,1,1.5,0 },
                new[] {1.0,2,3},
                1.0
            },
            new object[]
            {
                3,
                new[] { 0,-1,0.5,0 },
                new[] {1.0,2,3},
                -0.5
            },
            new object[]
            {
                3,
                new[] { 0,-1,0.5,1 },
                new[] {1.0,2,3},
                0.5
            },
            new object[]
            {
                3,
                new[] { 1.0,1,-1,0 },
                new[] {1.0,2,3},
                0.0
            }
        };
        public static readonly object[] IdentityNeuronTestsNc =
        {
            new object[]
            {
                3,
                new[] { 0,1,1.5 },
                new[] {1.0,2,3},
                1
            },
            new object[]
            {
                3,
                new[] { 0,-1,0.5 },
                new[] {1.0,2,3},
                -0.5
            }
        };

        public static ILayer<double> ConstantLayer_IdentityNeuron
        {
            get
            {
                Layer<double> layer = new Layer<double>(4,3);
                List<NeuronBase<double>> neurons = new List<NeuronBase<double>>();
                foreach (var neuronData in IdentityNeuronTestsC)
                {
                    IdentityNeuron neuron = new IdentityNeuron(3) {Weights = (double[])((object[])neuronData)[1]};
                    neurons.Add(neuron);
                }
                layer.Neurons = neurons.ToArray();
                return layer;
            }
            set { }
        }
    }
}