using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Utility.Dispose
{
    public static class DisposeHelper
    {
        public static void DisposeAndNull(IDisposable disposableObj)
        {
            disposableObj.Dispose();
            disposableObj = null;
        }
    }
}
