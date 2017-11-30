using NUnit.Framework;
using NUnit.Framework.Internal;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;

namespace NeuralNetwork.Core.Test.Helpers.Builder
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void TestBuilderEmpty()
        {
            var result = new Core.Helpers.Gen.Builder()
                .SetInput(3)
                .AddLayer(3, typeof(IdentityNeuron))
                .AddLayer(3, typeof(IdentityNeuron))
                .GetEmpty();
        }
    }
}