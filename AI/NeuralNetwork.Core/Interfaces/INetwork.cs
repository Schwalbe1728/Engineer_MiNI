namespace NeuralNetwork.Core.Interfaces
{
    public interface INetwork<T>
    {
        T[] Calculate(T[] input);
    }
}