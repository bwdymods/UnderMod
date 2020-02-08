namespace UnderMod.Internals
{
    internal static class Patches
    {

        internal static void Install()
        {
            API.instance.GetLogger().Info("Installing UnderMod Harmony patches...");

            API.instance.GetPatcher().Patch(typeof(Patches), typeof(Thor.Game), "Update", null, "Update");
            API.instance.GetPatcher().Patch(typeof(Patches), typeof(Thor.SimulationPlayer), "SpawnAvatar", null, "SpawnAvatar");
            API.instance.GetPatcher().Patch(typeof(Patches), typeof(Thor.SimulationPlayer), "DestroyAvatar", null, "DestroyAvatar");
            API.instance.GetPatcher().Patch(typeof(Patches), typeof(Thor.Simulation), "LoadZone", null, "LoadZone");
            API.instance.GetPatcher().Patch(typeof(Patches), typeof(Thor.CrashReporter), "OnSendClicked", "OnSendClicked", null);
            API.instance.GetPatcher().Patch(typeof(Patches), typeof(Thor.CrashReporter), "OnCrash", "OnCrash", null);
            API.instance.GetPatcher().Patch(typeof(Patches), typeof(Thor.TitlePopup), "Initialize", null, "Initialize");

            API.instance.GetLogger().Info("Harmony patches installed.");
        }

        internal static void Initialize(Thor.TitlePopup __instance, object data, Thor.Entity owner)
        {
            var t = UnderModAPI.Reflector.GetField<Thor.LocalizedText>(__instance, "m_versionText");
            t.Text = "UnderMine " + t.Text + " with UnderMod " + API.instance.GetAPIVersion().ToString();
        }

        internal static bool OnCrash(Thor.CrashReporter __instance, BugSplat.Report report)
        {
            API.instance.GetLogger().Fatal("The game has crashed: " + report.stacktrace);
            UnityEngine.Application.Quit();
            return false; //block execution of the original
        }

        internal static bool OnSendClicked(Thor.CrashReporter __instance)
        {
            return false; //block execution of the original
        }

        internal static void LoadZone(Thor.Simulation __instance)
        {
            //API.instance.GetLogger().Warn("zone changed");
        }

        internal static void SpawnAvatar(Thor.SimulationPlayer __instance)
        {
            //called when a game is loaded, and when a new avatar is spawned after death
            API.instance.GetEvents().AvatarSpawned(__instance);
        }

        internal static void DestroyAvatar(Thor.SimulationPlayer __instance)
        {
            //called when an avatar is destroyed. oddly, they use this to roll new avatar details, in addition to avatar spawning.
            API.instance.GetEvents().AvatarDestroyed(__instance);
        }

        internal static void Update()
        {
            //API.instance.GetLogger().Warn("awaked");
        }
    }
}
