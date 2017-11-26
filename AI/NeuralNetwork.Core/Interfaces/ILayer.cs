namespace NeuralNetwork.Core.Interfaces
{
    public interface ILayer<T>
    {
        T[] Calculate(T[] input);
    }
}