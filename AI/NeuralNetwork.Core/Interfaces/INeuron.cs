namespace NeuralNetwork.Core.Interfaces
{
    public interface INeuron<T>
    {
        T Process(T[] input);
        T[] GetWeights();
        void SetWeights(T[] weights);
        int GetCount();
    }
}