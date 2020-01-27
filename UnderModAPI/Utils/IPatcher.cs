using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderModAPI.Utils
{
    public interface IPatcher
    {
        void Patch(object caller, string typeColonMethodName, string prefixMethodName, string postfixMethodName, Type[] parameters = null);
        void Patch(object caller, Type targetType, string targetMethodName, string prefixMethodName, string postfixMethodName, Type[] parameters = null);
    }
}
