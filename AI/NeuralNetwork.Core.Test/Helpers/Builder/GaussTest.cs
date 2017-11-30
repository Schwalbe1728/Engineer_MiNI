using System;
using System.Linq;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;
using NUnit.Framework;
using Troschuetz.Random;

namespace NeuralNetwork.Core.Test.Helpers.Builder
{
    [TestFixture]
    public class GaussTest
    {
        [Test]
        public void Test()
        {

            var builder = new Core.Helpers.Gen.Builder();
            var network = builder.SetInput(3).AddLayer(5, typeof(IdentityNeuron))
                .AddLayer(2, typeof(IdentityNeuron)).GetEmpty().Randomize();
            var w = network.Layers[0].Neurons[0].GetWeights();
            w[0] = 0.0;
        }
    }
}