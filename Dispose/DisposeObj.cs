using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Utility.Dispose
{
    [Serializable]
    public abstract class DisposeObj : DisposeBase
    {
        protected bool _isDisposed = false;

        protected override void _Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    DisposeResources();
                }
            }
            _isDisposed = true;
        }

        protected abstract void DisposeResources();

        //public void Close()
        //{
        //    Dispose();
        //}
    }
}
