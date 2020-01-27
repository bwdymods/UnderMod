using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI.Objects
{
    public interface IAvatarInstance
    {
        object GetSimulationPlayer();
        void SetAvatarName(string name);
    }
}
