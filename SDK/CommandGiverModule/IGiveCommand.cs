using EngPlayerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandGiverModule
{
    public interface IGiveCommand
    {
        void GiveCommand(Command command);
    }
}
