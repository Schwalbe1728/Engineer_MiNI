using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using NeuralNetwork.Core.Interfaces;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;

namespace NeuralNetwork.Core.Helpers
{
    public static class ActivationFunctions
    {
        public static Dictionary<string,Func<double,double>> Functions = new Dictionary<string, Func<double, double>>
        {
            { "logistic", Logistic },
            { "identity", Identity },
            { "binary", Binary },
            { "tanH", TanH }
        };

        public static double Logistic(double input)
        {
            var result = 1.0 / (1.0 + Math.Exp(-input));
            return result;
        }
        public static double Identity(double input)
        {
            return input;
        }
        public static double Binary(double input)
        {
            var result = (input >= 0) ? 1 : 0;
            return result;
        }
        public static double TanH(double input)
        {
            var result = Math.Tanh(-input);
            return result;
        }

        public static INeuron<double> GetNeuron(string activation, int count, bool hasConstant = true)
        {
            switch (activation)
            {
                case "logistic":
                    return new IdentityNeuron(count,hasConstant);

            }
            return null;
        }
    }
}