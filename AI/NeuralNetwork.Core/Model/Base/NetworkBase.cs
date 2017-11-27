using System.Linq;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model
{
    public abstract class NetworkBase<T> : INetwork<T>
    {
        internal int LayerCount;
        internal int[] LayersCount;
        internal int InputCount;
        internal int OutputCount;

        public LayerBase<T>[] Layers { get; set; }

        protected NetworkBase() { }

        protected NetworkBase(LayerBase<T>[] layers)
        {
            Layers = layers;
            LayerCount = layers.Length;
            LayersCount = new int[LayerCount];
            for (int i = 0; i < LayerCount; i++)
                LayersCount[i] = Layers[i].Neurons.Length;
            InputCount = layers[0].InputCount;
            OutputCount = layers.Last().NeuronCount;
        }

        public T[] Calculate(T[] input)
        {
            foreach (var layer in Layers)
            {
                input = layer.Calculate(input);
            }
            return input;
        }
    }
}