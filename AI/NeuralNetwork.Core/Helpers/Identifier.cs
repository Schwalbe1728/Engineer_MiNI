using NeuralNetwork.Core.Model;

namespace NeuralNetwork.Core.Helpers
{
    public static class Identifier
    {
        public static double Identify(this NetworkBase<double> network)
        {
            double sum = 0;
            foreach (var layer in network.Layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    var weights = neuron.GetWeights();
                    for (var i = 0; i < weights.Length; i++)
                    {
                        sum += weights[i];
                    }
                }
            }
            return sum;
        }
    }
}