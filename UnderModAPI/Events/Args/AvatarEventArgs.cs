using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI.Events.Args
{
    public class AvatarEventArgs
    {
        public Objects.IAvatarInstance AvatarInstance;
        public AvatarEventArgs(Objects.IAvatarInstance avatar)
        {
            AvatarInstance = avatar;
        }
    }
}
