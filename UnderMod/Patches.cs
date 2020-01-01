using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

internal static class Patches
{

    internal static void Install()
    {
        UnderModAPI.Logger.Info("Installing UnderMod Harmony patches...");
        UnderModAPI.Patcher.Harmony = HarmonyInstance.Create("undermod.patches");

        UnderModAPI.Patcher.Patch(typeof(Patches), "Thor.Game:Update", null, "Update");
        UnderModAPI.Patcher.Patch(typeof(Patches), "Thor.Game:Awake", null, "Awake");
        UnderModAPI.Patcher.Patch(typeof(Patches), "Thor.SimulationPlayer:SpawnAvatar", null, "SpawnAvatar");
        
        UnderModAPI.Logger.Info("Harmony patches installed.");
    }

    internal static void SpawnAvatar(Thor.SimulationPlayer __instance)
    {
        //called when a game is loaded, and when a new avatar is spawned after death
        UnderModAPI.Logger.Alert("spawned avatar!");
        UnderModAPI.Events.AvatarSpawned(__instance);
    }

    internal static void Awake()
    {
        //called when the game is awakened, usually right after the intro video clips
        UnderModAPI.Logger.Alert("awakened!");
    }

    internal static void Update()
    {
        //UnderModAPI.Logger.Warn("awaked");
    }
}
