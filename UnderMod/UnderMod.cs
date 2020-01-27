using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnderModAPI;

namespace UnderMod
{
    public static class UnderMod
    {
        public static string GetGameDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(path)).FullName).FullName).FullName).FullName;
        }

        internal static void LoadMods()
        {
            string modDir = Path.Combine(GetGameDirectory(), "Mods");
            UnderModAPI.UnderMod.GetAPI().GetLogger().Info("Searching for mods in " + modDir);
            string[] directories = Directory.GetDirectories(modDir);
            List<ModData> modDatas = new List<ModData>();

            foreach (var dir in directories)
            {
                string modJsonPath = Path.Combine(dir, "mod.json");
                if (File.Exists(modJsonPath))
                {
                    ModData d = new ModData(modJsonPath);
                    if (d.Valid)
                    {
                        modDatas.Add(d);
                    }
                }
            }

            UnderModAPI.UnderMod.GetAPI().GetLogger().Info("Loading " + modDatas.Count + " mods...");
            foreach (var md in modDatas)
            {
                md.Load();
            }
            UnderModAPI.UnderMod.GetAPI().GetLogger().Info("Finished loading mods.");
        }
    }
}
