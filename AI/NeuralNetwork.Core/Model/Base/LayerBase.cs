using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model
{
    public abstract class LayerBase<T> : ILayer<T>
    {
        internal int NeuronCount;
        internal int InputCount;

        public INeuron<T>[] Neurons { get; set; }

        protected LayerBase(int neuronCount, int inputCount)
        {
            NeuronCount = neuronCount;
            InputCount = inputCount;
            Neurons = new INeuron<T>[NeuronCount];
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
    }
}