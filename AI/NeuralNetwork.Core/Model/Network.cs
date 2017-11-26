using System.Linq;
using NeuralNetwork.Core.Interfaces;

namespace NeuralNetwork.Core.Model
{
    public class Network<T> : NetworkBase<T>
    {
        public Network() { }
        public Network(LayerBase<T>[] layers) : base(layers) { }
    }
}