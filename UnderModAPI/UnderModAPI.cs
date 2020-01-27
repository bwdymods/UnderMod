using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI
{
    /// <summary>
    /// Static instance access for UnderMod API instance.
    /// </summary>
    public static class UnderMod
    {
        //used internally via reflection
        private static UnderModAPI API = null;

        /// <summary>
        /// Get an API instance. This will be the entrypoint for most access to the API.
        /// </summary>
        /// <returns></returns>
        public static UnderModAPI GetAPI()
        {
            return API;
        }
    }

    /// <summary>
    /// Primary API root for UnderMod API.
    /// </summary>
    public interface UnderModAPI
    {
        /// <summary>
        /// Get the version of the API that is loaded.
        /// </summary>
        /// <returns>Version struct representing the version of the UnderMod API that is loaded.</returns>
        Structs.Version GetAPIVersion();

        /// <summary>
        /// Get the Thor.Game instance that is running currently.
        /// Generic object avoids mandatory referencing of game internals. Cast to Thor.Game if desired.
        /// </summary>
        /// <returns>Thor.Game instance as a generic object</returns>
        object GetGameInstance();

        /// <summary>
        /// Gets the root directory of the game, where UnderMine.exe is located.
        /// </summary>
        /// <returns>A string fullpath of the root directory of UnderMine's installation.</returns>
        string GetGameDirectory();

        /// <summary>
        /// Gets the logger instance currently employed by UnderMod.
        /// </summary>
        /// <returns>An object implementing UnderModAPI.Utils.ILogger</returns>
        Utils.ILogger GetLogger();

        /// <summary>
        /// Gets the patcher instance currently employed by UnderMod.
        /// </summary>
        /// <returns>An object implementing UnderModAPI.Utils.IPatcher</returns>
        Utils.IPatcher GetPatcher();
    }
}
