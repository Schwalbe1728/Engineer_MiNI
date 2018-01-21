using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Learning.Enums;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;
using NUnit.Framework;

namespace NeuralNetwork.Core.Test.SerializatorTests
{
    [TestFixture]
    public class GeneticAlgorithmSerialization
    {
        [Test]
        public void ProcessDataSerialization()
        {
            var data = GetProcessData();

            XmlSerializer ser = new XmlSerializer(typeof(ProcessData));

            var obj = GetProcessData();

            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj, result);
        }

        [Test]
        public void RandomizerOptionsSerialization()
        {
            var data = GetProcessData();

            XmlSerializer ser = new XmlSerializer(typeof(RandomizerOptions));

            var obj = GetRandomizerOptions();

            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj, result);
        }

        [Test]
        public void GeneticAlgorithmConfigSerialization()
        {
            var data = GetProcessData();

            XmlSerializer ser = new XmlSerializer(typeof(GeneticAlgorithmConfig));

            var obj = GetGeneticAlgorithmConfig();

            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj, result);
        }

        [Test]
        public void GeneticAlgorithmSerializationTest()
        {
            var data = GetProcessData();

            XmlSerializer ser = new XmlSerializer(typeof(GeneticAlgorithm));

            var obj = GetGeneticAlgorithm();

            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj, result);
        }
        [Test]
        public void LearningProcessSerializationTest()
        {
            var data = GetProcessData();

            XmlSerializer ser = new XmlSerializer(typeof(LearningProcess));

            var obj = GetLearningProcess();

            MemoryStream stream = new MemoryStream();
            ser.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ser.Deserialize(stream);

            Assert.AreEqual(obj, result);
        }

        #region Generate
        public NetworkBase<double> GetNetwork()
        {
            return Builder.GetBuilder(new List<int>() { 2, 4, 2 },
                new List<Type>() { typeof(TanHNeuron), typeof(IdentityNeuron) }).GetEmpty().Randomize();
        }
        public ProcessData GetProcessData()
        {
            var data = new ProcessData()
            {
                AverageScore = 2,
                BestScore = 3,
                BestSpecimen = GetNetwork(),
                GenerationIndex = 4,
                MedianScore = 5,
                Timestamp = DateTime.Now,
                WorstScore = 6
            };
            return data;
        }
        public RandomizerOptions GetRandomizerOptions()
        {
            var result = new RandomizerOptions()
            {
                Max = 2,
                Mean = 3,
                Min = 4,
                Sigma = 6
            };
            return result;
        }
        public GeneticAlgorithmConfig GetGeneticAlgorithmConfig()
        {
            var result = new GeneticAlgorithmConfig()
            {
                MutationChance = 1,
                ParentMethod = ParentChoosingMethod.Geom,
                PercentToSelect = 2,
                RandOptions = GetRandomizerOptions()
            };
            return result;
        }
        public GeneticAlgorithm GetGeneticAlgorithm()
        {
            var result = new GeneticAlgorithm()
            {
                Config = GetGeneticAlgorithmConfig(),
                CanSelfReproduce = true
            };
            return result;
        }

        public LearningProcess GetLearningProcess()
        {
            var result = new LearningProcess()
            {
                BestIndex = 1,
                Generation = 2,
                HistoricalData = new List<ProcessData>() { GetProcessData()},
                LearningAlgorithm = GetGeneticAlgorithm(),
                Population = new [] {GetNetwork()},
                PopulationCount = 3
            };
            return result;
        }
        #endregion
    }
}