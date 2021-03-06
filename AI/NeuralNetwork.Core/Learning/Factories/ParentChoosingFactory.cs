﻿using System;
using System.Linq;
using NeuralNetwork.Core.Learning.Enums;

namespace NeuralNetwork.Core.Learning.Factories
{
    public static class ParentChoosingFactory
    {
        public static Func<int, double[], int[]> Get(ParentChoosingMethod method)
        {
            switch (method)
            {
                case ParentChoosingMethod.PositionLinear:
                    return PositionLinear;
                case ParentChoosingMethod.ScoreLinear:
                    return ScoreLinear;
                case ParentChoosingMethod.ScoreCubic:
                    return ScoreCubic;
                case ParentChoosingMethod.PositionCubic:
                    return PositionCubic;
                case ParentChoosingMethod.PositionExponential:
                    return PositionExponential;
                case ParentChoosingMethod.Geom:
                    return Geom;
            }
            return PositionLinear;
        }

        public static int[] PositionLinear(int count, double[] scores)
        {
            int[] result = new int[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = count - i;
            }
            return result;
        }

        public static int[] ScoreLinear(int count, double[] scores)
        {
            int[] result = new int[count];

            double min = scores.Last();
            double max = scores.First();
            for (int i = 0; i < count; i++)
            {
                scores[i] = scores[i] + min;
            }

            for (int i = 0; i < count; i++)
            {
                result[i] = (int) ((scores[i] * 1000) / max);
            }
            return result;
        }

        public static int[] ScoreCubic(int count, double[] scores)
        {
            int[] result = new int[count];

            double min = scores.Last();
            double max = scores.First();
            for (int i = 0; i < count; i++)
            {
                scores[i] = scores[i] + min;
            }

            for (int i = 0; i < count; i++)
            {
                result[i] = (int) Math.Pow((scores[i] * 1000) / max, 1.33333333);
            }
            return result;
        }

        public static int[] PositionCubic(int count, double[] scores)
        {
            int[] result = new int[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = (int) Math.Pow(count - i, 2);
            }
            return result;
        }
        public static int[] PositionExponential(int count, double[] scores)
        {
            int[] result = new int[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = (count - i) * 2 ^ (count - i);
            }
            return result;
        }

        public static int[] Geom(int count, double[] scores)
        {
            int total = 1000000;
            int[] result = new int[count];
            for (int i = 0; i < count; i++)
            {
                total = total / 2;
                result[i] = total;
            }
            return result;
        }
    }
}