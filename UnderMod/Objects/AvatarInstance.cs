using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderMod.Objects
{
    public class AvatarInstance : UnderModAPI.Objects.IAvatarInstance
    {
        Thor.SimulationPlayer avatar;

        public AvatarInstance(Thor.SimulationPlayer instance)
        {
            avatar = instance;
        }

        public object GetSimulationPlayer()
        {
            return avatar;
        }

        public void SetAvatarName(string name)
        {
            if(avatar != null && avatar.Avatar != null) avatar.Avatar.name = name;
            Thor.GameData.Instance.SetPeonName(name);
        }
    }
}
