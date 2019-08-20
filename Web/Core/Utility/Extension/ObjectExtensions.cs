using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Extension
{
    public static class ObjectExtensions
    {
        public static string TryString(this object value)
        {
            return value != null ? value.ToString() : "";
        }
    }
}
