using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI
{
    public static class Events
    {
        public static event EventHandler<AvatarSpawnedEventArgs> OnAvatarSpawned;
        internal static void AvatarSpawned(Thor.SimulationPlayer player)
        {
            OnAvatarSpawned(null, new UnderModAPI.AvatarSpawnedEventArgs(player));
        }
    }

    public class AvatarSpawnedEventArgs : EventArgs
    {
        public Thor.SimulationPlayer Player { get; }
        public AvatarSpawnedEventArgs(Thor.SimulationPlayer player)
        {
            Player = player;
        }
    }
}
