using System;
using System.Linq;
using System.Xml.Serialization;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model
{
    [XmlInclude(typeof(Network<double>))]
    [Serializable]
    public abstract class NetworkBase<T> : INetwork<T>
    {
        public int LayerCount;
        public int[] LayersCount;
        public int InputCount;
        public int OutputCount;

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

        public override bool Equals(object obj)
        {
            if (!(obj is NetworkBase<T>))
                return false;
            var tmp = (NetworkBase<T>)obj;
            return obj.GetType() == GetType()
                   && LayerCount == tmp.LayerCount
                   && LayersCount.SequenceEqual(tmp.LayersCount)
                   && InputCount == tmp.InputCount
                   && OutputCount == tmp.OutputCount
                   && Layers.SequenceEqual(tmp.Layers);
        }
    }
}