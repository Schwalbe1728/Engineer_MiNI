using System.Linq;
using NeuralNetwork.Core.Test.Setup;
using NUnit.Framework;

namespace NeuralNetwork.Core.Test.Model
{
    [TestFixture]
    public class LayerTests
    {
        private double[] _result;
        [SetUp]
        public void Init()
        {
            _result = Values.IdentityNeuronTestsC
                .Select(x => (object[]) x)
                .Select(x => (double)x[3])
                .ToArray();

        }
        [Test]
        public void LayerTest()
        {
            var layer = Values.ConstantLayer_IdentityNeuron;
            var result = layer.Calculate(new[] {1.0, 2, 3});
            Assert.AreEqual(_result,result);
        }
    }
}