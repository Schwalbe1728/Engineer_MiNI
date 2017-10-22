using CommandGiverModule;
using DataAcquiringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindAIToUnityModule
{
    public interface IAIUnityBinder
    {
        void SetCommandGiver(IGiveCommand comm);
        void SetDataReceiver(IAcquireData data);
    }
}
