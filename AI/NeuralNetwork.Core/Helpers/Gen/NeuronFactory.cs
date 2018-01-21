using System;
using System.Collections.Generic;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;

namespace NeuralNetwork.Core.Helpers.Gen
{
    public static class NeuronFactory
    {
        static Dictionary<Type, Func<int, bool, NeuronBase<double>>> types = new Dictionary<Type, Func<int,bool, NeuronBase<double>>> {
            { typeof(IdentityNeuron), (x,y) => new IdentityNeuron(x,y) },
            { typeof(StepNeuron), (x,y) => new StepNeuron(x,y) },
            { typeof(TanHNeuron), (x,y) => new TanHNeuron(x,y) }
        };
        public static NeuronBase<double> Get(Type t,int inputCount, bool hasConstant = true)
        {
            if (types.ContainsKey(t))
                return types[t](inputCount,hasConstant);
            throw new ArgumentException("Type " + t + " not found");
        }
    }
}