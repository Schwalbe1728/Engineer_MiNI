using System;
using System.Linq;
using System.Xml.Serialization;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model.Neurons;

namespace NeuralNetwork.Core.Model
{
    [XmlInclude(typeof(TanHNeuron))]
    [XmlInclude(typeof(IdentityNeuron))]
    [XmlInclude(typeof(StepNeuron))]
    [Serializable]
    public abstract class NeuronBase<T> : INeuron<T>
    {
        public int InputCount;
        public bool HasConstant;
        public T[] Weights { get; set; }

        public NeuronBase() { }
        public NeuronBase(int inputCount, bool hasConstant = true)
        {
            InputCount = inputCount;
            HasConstant = hasConstant;

            if (HasConstant)
                Weights = new T[InputCount + 1];
            else
                Weights = new T[InputCount];
        }

        public abstract T Process(T[] input);

        public virtual T[] GetWeights()
        {
            return Weights;
        }

        public virtual void SetWeights(T[] weights)
        {
            Weights = weights;
            if (HasConstant)
                InputCount = Weights.Length - 1;
            else
                InputCount = Weights.Length;
        }

        public virtual int GetCount()
        {
            return Weights.Length;
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is NeuronBase<T>))
                return false;
            var tmp = (NeuronBase<T>)obj;
            return obj.GetType() == GetType()
                   && InputCount == tmp.InputCount
                   && HasConstant == tmp.HasConstant
                   && Weights.SequenceEqual(tmp.Weights);
        }
    }
}