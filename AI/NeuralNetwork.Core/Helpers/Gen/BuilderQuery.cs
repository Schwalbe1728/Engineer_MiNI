using System;
using System.Collections.Generic;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model;

namespace NeuralNetwork.Core.Helpers.Gen
{
    public class BuilderQuery
    {
        private Gen.Builder _builder;

        public BuilderQuery(Gen.Builder builder)
        {
            _builder = builder;
        }
        
        public BuilderQuery AddLayer(int count, Type neuron)
        {
            _builder.LayerCount.Add(count);
            _builder.Neurons.Add(neuron);
            return this;
        }

        public NetworkBase<double> GetEmpty()
        {
            var tmpInput = _builder.Input;
            List<LayerBase<double>> layers = new List<LayerBase<double>>();
            for (int j = 0; j < _builder.LayerCount.Count; j++)
            {
                var l = new Layer<double>(_builder.LayerCount[j], tmpInput);
                for (int i = 0; i < _builder.LayerCount[j]; i++)
                {
                    l.Neurons[i] = NeuronFactory.Get(_builder.Neurons[j], tmpInput);
                }
                tmpInput = _builder.LayerCount[j];
                layers.Add(l);
            }
            NetworkBase<double> result = new Network<double>(layers.ToArray());
            return result;
        }
    }
}