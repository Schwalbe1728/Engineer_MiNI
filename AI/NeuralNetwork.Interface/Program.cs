using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Interface.Examples;

namespace NeuralNetwork.Interface
{
    class Program
    {
        static void Main(string[] args)
        {
            //var ex1 = new Perceptron();
            //ex1.Run();
            //var ex2 = new Classification();
            //ex2.Run();
            var ex3 = new Load();
            ex3.Run();
        }
    }
}
