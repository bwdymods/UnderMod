using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderModAPI.Events.Args;

namespace UnderMod.Events
{
    public class Events : UnderModAPI.Events.IEvents
    {
        public event EventHandler<AvatarEventArgs> OnAvatarSpawned;
        public event EventHandler<AvatarEventArgs> OnAvatarDestroyed;

        internal void AvatarSpawned(Thor.SimulationPlayer player)
        {
            var i = new Objects.AvatarInstance(player);
            if(OnAvatarSpawned != null) OnAvatarSpawned(null, new AvatarEventArgs(i));
        }

        internal void AvatarDestroyed(Thor.SimulationPlayer player)
        {
            var i = new Objects.AvatarInstance(player);
            if(OnAvatarDestroyed != null) OnAvatarDestroyed(null, new AvatarEventArgs(i));
        }

        public event EventHandler<DataInitializedEventArgs> OnDataInitialized;

        internal void DataInitialized()
        {
            if(OnDataInitialized != null) OnDataInitialized(null, new DataInitializedEventArgs());
        }
    }
}
