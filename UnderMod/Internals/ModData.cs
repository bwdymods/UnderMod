using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using UnderModAPI;
using System.Runtime.Serialization;
using System.Reflection;
using System.Linq;

internal class ModData
{
    internal bool Valid = false;
    internal string Directory;
    internal string Name;
    internal string Tagline;
    internal string Author;
    internal string DLL;
    internal UnderModAPI.Structs.Version Version;
    internal UnderModAPI.Structs.Version API;
    internal Assembly Assembly;
    internal IMod Mod;

    internal ModData(string jsonPath)
    {
        try
        {
            Directory = Path.GetDirectoryName(jsonPath);
            string json = File.ReadAllText(jsonPath);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(ModDataContract));
                ModDataContract mod = (ModDataContract)serializer.ReadObject(ms);
                Name = mod.Name;
                Author = mod.Author;
                Tagline = mod.Tagline;
                DLL = mod.DLL;
                Version = new UnderModAPI.Structs.Version(mod.Version);
                API = new UnderModAPI.Structs.Version(mod.API);
                if(API > UnderMod.API.instance.GetAPIVersion())
                {
                    UnderMod.API.instance.GetLogger().Warn("Skipping mod described by " + jsonPath + " because it requires a newer API version: " + API.ToString());
                    return;
                } else if (API < new UnderModAPI.Structs.Version("1.1"))
                {
                    UnderMod.API.instance.GetLogger().Warn("Skipping mod described by " + jsonPath + " because it is not compatible with this version of UnderMod.");
                    return;
                }
            }
            Valid = true;
        } catch (Exception e)
        {
            UnderMod.API.instance.GetLogger().Warn("Skipping mod described by " + jsonPath + " because it is invalid:\n" + e.Message);
        }
    }

    internal void Load()
    {
        try
        {
            Assembly = Assembly.LoadFrom(Path.Combine(Directory, DLL));

            bool entry = false;
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterfaces().Contains(typeof(IMod)))
                {
                    Mod = (IMod)Activator.CreateInstance(t);
                    entry = true;
                    UnderMod.API.instance.GetLogger().Info("Loading mod: " + Name + " " + Version + " by " + Author + " | " + Tagline);
                    Mod.OnEntry();
                    break;
                }
            }
            if (!entry)
            {
                UnderMod.API.instance.GetLogger().Warn("Mod " + Name + " does not contain an entrypoint (implementation of UnderModAPI.Mod).");
            }
        } catch(Exception e)
        {
            UnderMod.API.instance.GetLogger().Error("Mod " + Name + " threw an error during OnEntry: " + e.Message);
        }
    }
    [DataContract]
    class ModDataContract
    {
        [DataMember(Name = "Name")] public string Name { get; set; }
        [DataMember(Name = "Author")] public string Author { get; set; }
        [DataMember(Name = "Version")] public string Version { get; set; }
        [DataMember(Name = "Tagline")] public string Tagline { get; set; }
        [DataMember(Name = "DLL")] public string DLL { get; set; }
        [DataMember(Name = "API")] public string API { get; set; }
    }
}

