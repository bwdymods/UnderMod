using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI
{
    public static class Reflector
    {
        private const BindingFlags BindingFlagsAll = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        public static T GetField<T>(object instance, string name, Type type = null)
        {
            Type t = (type == null ? instance.GetType() : type);
            return (T)t.GetField(name, BindingFlagsAll).GetValue(instance);
        }

        public static void SetField(object instance, string name, object value, Type type = null)
        {
            Type t = (type == null ? instance.GetType() : type);
            t.GetField(name, BindingFlagsAll).SetValue(instance, value);
        }

        //parameterless invocation with return
        public static T Invoke<T>(object instance, string name, Type type = null)
        {
            Type t = (type == null ? instance.GetType() : type);
            return (T) t.GetMethod(name, BindingFlagsAll).Invoke(instance, null);
        }

        //parameterless invocation without return (void)
        public static void Invoke(object instance, string name, Type type = null)
        {
            Type t = (type == null ? instance.GetType() : type);
            t.GetMethod(name, BindingFlagsAll).Invoke(instance, null);
        }
    }
}
