using System;
using System.Collections.Generic;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Helpers.Gen
{
    public class Builder
    {
        public List<int> LayerCount;
        public List<Type> Neurons;
        public int Input;

        public Builder()
        {
            LayerCount = new List<int>();
            Neurons = new List<Type>();
        }

        public BuilderQuery SetInput(int input)
        {
            Input = input;
            return new BuilderQuery(this);
        }

        public static BuilderQuery GetBuilder(List<int> layerCount, List<Type> neuronTypes)
        {
            if(layerCount.Count < 2)
                throw new ArgumentException("LayerCount needs at least two arguments: input and output layer.");
            if(neuronTypes.Count != layerCount.Count - 1)
                throw new ArgumentException("NeuronTypes should have (LayerCount - 1) elements");

            var builder = new Builder().SetInput(layerCount[0]);

            for (int i = 1; i < layerCount.Count; i++)
                builder.AddLayer(layerCount[i], neuronTypes[i - 1]);

            return builder;
        }
    }
}