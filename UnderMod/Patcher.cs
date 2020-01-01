using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI
{
    public static class Patcher
    {
        internal static HarmonyInstance Harmony;
        public static void Patch(object caller, string typeColonMethodName, string prefixMethodName, string postfixMethodName)
        {
            MethodInfo targetmethod = AccessTools.Method(typeColonMethodName);
            if (targetmethod == null)
            {
                UnderModAPI.Logger.Error("Failed to located target method!");
            }
            HarmonyMethod pre = null;
            Type callType = (caller is Type) ? (Type)caller : caller.GetType();
            if (prefixMethodName != null)
            {
                pre = new HarmonyMethod(caller.GetType().GetMethod(prefixMethodName, BindingFlags.NonPublic | BindingFlags.Static));
            }
            HarmonyMethod post = null;
            if (postfixMethodName != null)
            {
                post = new HarmonyMethod(caller.GetType().GetMethod(postfixMethodName, BindingFlags.NonPublic | BindingFlags.Static));
            }
            Harmony.Patch(targetmethod, pre, post);
        }
    }
}
