using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderMod
{
    public class API : UnderModAPI.UnderModAPI
    {
        //internal stuff 

        private UnderModAPI.Utils.ILogger Logger;
        private UnderModAPI.Utils.IPatcher Patcher;
        internal static API instance;
        private Events.Events Events;
        private bool setup = false;

        internal API()
        {
            instance = this;
        }

        internal void Setup()
        {
            if (!setup)
            {
                Events = new Events.Events();
                Logger = new Utils.Logger();
                Patcher = new Utils.Patcher();
                setup = true;
            }
        }

        internal Events.Events GetEvents()
        {
            Setup();
            return Events;
        }



        //public stuff
        public string GetGameDirectory()
        {
            return UnderMod.GetGameDirectory();
        }

        public UnderModAPI.Structs.Version GetAPIVersion()
        {
            return new UnderModAPI.Structs.Version(1, 2, 0, 0);
        }

        public object GetGameInstance()
        {
            return Thor.Game.Instance;
        }

        public UnderModAPI.Utils.ILogger GetLogger()
        {
            Setup();
            return Logger;
        }

        public UnderModAPI.Utils.IPatcher GetPatcher()
        {
            Setup();
            return Patcher;
        }
    }
}
