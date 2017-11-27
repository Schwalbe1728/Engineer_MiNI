﻿using System;
using System.Collections.Generic;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model;

namespace NeuralNetwork.Core.Helpers.Gen
{
    public static class NeuronFactory
    {
        static Dictionary<Type, Func<int, bool, INeuron<double>>> types = new Dictionary<Type, Func<int,bool,INeuron<double>>> {
            { typeof(IdentityNeuron), (x,y) => new IdentityNeuron(x,y) }
        };
        public static INeuron<double> Get(Type t,int inputCount, bool hasConstant = true)
        {
            if (types.ContainsKey(t))
                return types[t](inputCount,hasConstant);
            throw new ArgumentException("Type " + t + " not found");
        }
    }
}