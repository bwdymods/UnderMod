using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnderModAPI;

public static class UnderMod
{
    private static bool RunOnce = false;
    [DllImport("Kernel32.dll")]
    private static extern bool AllocConsole();

    private static string AssemblyDirectory
    {
        get
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }

    private static string GameDirectory
    {
        get
        {
            return Directory.GetParent(Directory.GetParent(AssemblyDirectory).FullName).FullName;
        }
    }

    public static void Initialize()
    {
        if (!RunOnce)
        {
            RunOnce = true;

            //make sure it was launched by the launcher
            if (String.Join(", ", Environment.GetCommandLineArgs()).Contains("UnderModLauncher"))
            {
                //create a console window and hijack the output for our logger
                AllocConsole();
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
                UnderModAPI.Logger.Initialize();
                Patches.Install();
                LoadMods();
            }
        }
    }

    public static Thor.Game GetGameInstance()
    {
        return Thor.Game.Instance;
    }

    public static UnderModAPI.Version GetVersion()
    {
        return new UnderModAPI.Version(1, 0, 0, 0);
    }

    private static void LoadMods()
    {
        string modDir = Path.Combine(GameDirectory, "Mods");
        Logger.Info("Searching for mods in " + modDir);
        string[] directories = Directory.GetDirectories(modDir);
        List<ModData> modDatas = new List<ModData>();

        foreach(var dir in directories)
        {
            string modJsonPath = Path.Combine(dir, "mod.json");
            if(File.Exists(modJsonPath))
            {
                ModData d = new ModData(modJsonPath);
                if (d.Valid)
                {
                    modDatas.Add(d);
                }
            }
        }

        Logger.Info("Loading " + modDatas.Count + " mods...");
        foreach(var md in modDatas)
        {
            md.Load();
        }
        Logger.Info("Finished loading mods.");
    }
}
