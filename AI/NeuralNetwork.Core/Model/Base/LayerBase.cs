using System;
using System.Linq;
using System.Xml.Serialization;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model
{
    [XmlInclude(typeof(Layer<double>))]
    [Serializable]
    public abstract class LayerBase<T> : ILayer<T>
    {
        public int NeuronCount;
        public int InputCount;

        public NeuronBase<T>[] Neurons { get; set; }

        protected LayerBase(int neuronCount, int inputCount)
        {
            NeuronCount = neuronCount;
            InputCount = inputCount;
            Neurons = new NeuronBase<T>[NeuronCount];
        }

        protected LayerBase() { }

        public virtual T[] Calculate(T[] input)
        {
            var result = new T[NeuronCount];
            for (int i = 0; i < NeuronCount; i++)
            {
                result[i] = Neurons[i].Process(input);
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LayerBase<T>))
                return false;
            var tmp = (LayerBase<T>)obj;
            return obj.GetType() == GetType()
                   && InputCount == tmp.InputCount
                   && NeuronCount == tmp.NeuronCount
                   && Neurons.SequenceEqual(tmp.Neurons);
        }
    }
}