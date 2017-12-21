using EngPlayerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.AI
{
    public class SimplifiedCommand : Command
    {
        public CommandType ComType { get; private set; }

        public SimplifiedCommand(CommandType type)
        {
            ComType = type;
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
