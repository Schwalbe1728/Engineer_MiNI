using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Test.Setup;
using NUnit.Framework;

namespace NeuralNetwork.Core.Test.Model
{
    [TestFixture]
    public class NeuronTests
    {
        [Test, TestCaseSource(typeof(Values), "IdentityNeuronTestsC")]
        public void IdentityNeuron_wConstant_ProcessTest(int count, 
            double[] weights, double[] input, double output)
        {
            var neuron = new IdentityNeuron(count) {Weights = weights};
            var result = neuron.Process(input);
            Assert.AreEqual(output,result);
        }

        [Test, TestCaseSource(typeof(Values), "IdentityNeuronTestsNc")]
        public void IdentityNeuron_woConstant_ProcessTest(int count,
            double[] weights, double[] input, double output)
        {
            var neuron = new IdentityNeuron(count, false) {Weights = weights};
            var result = neuron.Process(input);
            Assert.AreEqual(output, result);
        }
    }
}