namespace NeuralNetwork.Core.Interfaces
{
    public interface INeuron<T>
    {
        T Process(T[] input);
        double[] GetWeights();
        void SetWeights(double[] weights);
        int GetCount();
    }
}