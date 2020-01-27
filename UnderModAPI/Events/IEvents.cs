using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI.Events
{
    public interface IEvents
    {
        event EventHandler<Args.AvatarEventArgs> OnAvatarSpawned;
        event EventHandler<Args.AvatarEventArgs> OnAvatarDestroyed;
        event EventHandler<Args.DataInitializedEventArgs> OnDataInitialized;
    }
}
