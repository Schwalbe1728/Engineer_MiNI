using System;
using System.Collections.Generic;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model;

namespace NeuralNetwork.Core.Helpers.Gen
{
    public static class NeuralHelper
    {
        public static NetworkBase<double> Randomize(this NetworkBase<double> network, RandomizerOptions options = null)
        {
            return Rand(network, options);
        }

        public static Network<double> Randomize(this Network<double> network, RandomizerOptions options = null)
        {
            return (Network<double>)Rand(network, options);
        }

        private static NetworkBase<double> Rand(NetworkBase<double> network, RandomizerOptions options = null)
        {
            if (options == null)
                options = new RandomizerOptions(-1, 1);

            foreach (var layer in network.Layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    var count = neuron.GetCount();
                    var weights = new double[count];
                    for (int i = 0; i < count; i++)
                        weights[i] = options.Uniform.NextDouble();
                    neuron.SetWeights(weights);
                }
            }

            return network;
        }

        public static NetworkBase<double> Copy(this NetworkBase<double> network)
        {
            var builder = new Builder().SetInput(network.InputCount);
            foreach (var layer in network.Layers)
            {
                builder.AddLayer(layer.NeuronCount, layer.Neurons[0].GetType());
            }
            var result = builder.GetEmpty();

            for (int i = 0; i < network.LayerCount; i++)
            {
                int count = network.Layers[i].InputCount;
                for (int k = 0; k < network.Layers[i].NeuronCount; k++)
                {
                    var w = network.Layers[i].Neurons[k].GetWeights();
                    var weights = new double[w.Length];
                    w.CopyTo(weights, 0);
                    result.Layers[i].Neurons[k].SetWeights(weights);
                }
            }
            return result;
        }
    }
}