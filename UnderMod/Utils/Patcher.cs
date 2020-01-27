using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnderModAPI.Utils;

namespace UnderMod.Utils
{
    public class Patcher : IPatcher
    {
        internal static HarmonyInstance Harmony = HarmonyInstance.Create("undermod.patches");
        public void Patch(object caller, string typeColonMethodName, string prefixMethodName, string postfixMethodName, Type[] parameters = null)
        {
            MethodInfo targetmethod = AccessTools.Method(typeColonMethodName, parameters);
            if (targetmethod == null)
            {
                API.instance.GetLogger().Error("Failed to located target method!");
            }
            Type callType = (caller is Type) ? (Type)caller : caller.GetType();
            DoPatch(callType, targetmethod, prefixMethodName, postfixMethodName);
        }

        public void Patch(object caller, Type targetType, string targetMethodName, string prefixMethodName, string postfixMethodName, Type[] parameters = null)
        {
            MethodInfo targetmethod = AccessTools.Method(targetType, targetMethodName, parameters);
            if (targetmethod == null)
            {
                API.instance.GetLogger().Error("Failed to located target method!");
            }
            Type callType = (caller is Type) ? (Type)caller : caller.GetType();
            DoPatch(callType, targetmethod, prefixMethodName, postfixMethodName);
        }

        private void DoPatch(Type callType, MethodInfo target, string prefixMethodName, string postfixMethodName)
        {
            HarmonyMethod pre = null;
            
            if (prefixMethodName != null)
            {
                pre = new HarmonyMethod(callType.GetMethod(prefixMethodName, BindingFlags.NonPublic | BindingFlags.Static));
                if (pre == null && prefixMethodName != null)
                {
                    API.instance.GetLogger().Error("Possible error installing prefix patch.");
                }
            }
            HarmonyMethod post = null;
            if (postfixMethodName != null)
            {
                post = new HarmonyMethod(callType.GetMethod(postfixMethodName, BindingFlags.NonPublic | BindingFlags.Static));
                if (post == null && postfixMethodName != null)
                {
                    API.instance.GetLogger().Error("Possible error installing postfix patch.");
                }
            }
            Harmony.Patch(target, pre, post);
            string p = "";
            if (pre != null) p += "prefix ";
            if (post != null) p += pre != null ? " and postfix" : "postfix";
            API.instance.GetLogger().Alert("Patched with " + p);
        }
    }
}
