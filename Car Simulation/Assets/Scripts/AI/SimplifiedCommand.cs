using EngPlayerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class SimplifiedCommand : Command
    {
        public CommandType ComType { get; private set; }
        private float value;
        
        public float Value { get { return Mathf.Max(0, Mathf.Min(value, 1)); } }        

        public SimplifiedCommand(CommandType type)
        {
            ComType = type;
        }

        public SimplifiedCommand(CommandType type, float val)
        {
            ComType = type;
            value = val;
        }
    }

    public enum CommandType
    {
        TurnRight,
        TurnLeft,
        Accelerate,
        Break
    }
}
