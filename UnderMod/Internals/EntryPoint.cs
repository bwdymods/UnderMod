using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using VortexHarmonyInstaller.ModTypes;

namespace UnderMod
{
    public class EntryPoint
    {
        private static bool RunOnce = false;
        private static bool Started = false;
        private static bool DataInitialized = false;


        [DllImport("Kernel32.dll")]
        private static extern bool AllocConsole();

        public static void OnGameAwake(VortexMod modInfo)
        {
            if (!RunOnce)
            {
                RunOnce = true;
                UnityEngine.Application.logMessageReceivedThreaded += UELog;
            }
        }

        private static void Start()
        {
            UnderModAPI.Reflector.SetField(null, "API", new API(), typeof(UnderModAPI.UnderMod));

            //create a console window and hijack the output for our logger
            AllocConsole();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OpenStandardOutput().Flush(); //ditch the vortex error on already installed
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.Title = "UnderMod " + API.instance.GetAPIVersion().ToString() + " :: UnderMine Mod API";
            //Internals.RelicMaker.Test();
            API.instance.GetLogger().Info("UnderMod API Initialized.");
            Internals.Patches.Install();
            UnderMod.LoadMods();
        }


        private static void UELog(string condition, string stackTrace, UnityEngine.LogType type)
        {
            string msg = condition;
            if (!string.IsNullOrWhiteSpace(stackTrace)) msg += " " + stackTrace;
            if (!Started)
            {
                if (msg.ToUpper().Contains("STEAM"))
                {
                    //by waiting to hook until steam is loaded, we skip the vortex errors for failed duplicate installation
                    Started = true;
                    Start();
                }
            }
            else if (!DataInitialized)
            {
                if (msg.ToUpper().Contains("DONE"))
                {
                    DataInitialized = true;
                    API.instance.GetLogger().Info("Data Initialized.");
                    API.instance.GetEvents().DataInitialized();
                    return;
                }
            }
            API.instance.GetLogger().Info(msg);
        }
    }
}
