using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace COM.Utility.Dispose
{
    [Serializable]
    public abstract class DisposeBase : IDisposable
    {
        ~DisposeBase()
        {
            _Dispose(false);
        }

        public void Dispose()
        {
            _Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void _Dispose(bool disposing);
    }
}
