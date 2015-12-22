using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Utility.Dispose
{
    public static class StaticDispose
    {
        public static void DisposeAndNull(object disposableObj)
        {
            if (disposableObj != null && disposableObj is IDisposable)
            {
                (disposableObj as IDisposable).Dispose();
                disposableObj = null;
            }
        }
    }
}
