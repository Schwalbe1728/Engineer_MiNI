using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Xml.Serialization;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;
using NUnit.Framework;

namespace NeuralNetwork.Core.Test.SerializatorTests
{
    [TestFixture]
    public class NeuralSerialization
    {
        public NetworkBase<double> GetNetwork()
        {
            return Builder.GetBuilder(new List<int>() {2, 4, 2},
                new List<Type>() {typeof(TanHNeuron), typeof(IdentityNeuron)}).GetEmpty().Randomize();
        }

        [Test]
        public void SerializeNeuron()
        {
            XmlSerializer ser = new XmlSerializer(typeof(NeuronBase<double>));

            var obj = GetNetwork().Layers[0].Neurons[0];
            
            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj,result);
        }

        [Test]
        public void SerializeLayer()
        {
            XmlSerializer ser = new XmlSerializer(typeof(LayerBase<double>));
            var obj = GetNetwork().Layers[0];

            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj, result);
        }

        [Test]
        public void SerializeNetwork()
        {
            XmlSerializer ser = new XmlSerializer(typeof(NetworkBase<double>));
            var obj = GetNetwork();

            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj, result);
        }
    }
}