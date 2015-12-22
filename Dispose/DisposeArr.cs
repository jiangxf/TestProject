using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Utility.Dispose
{
    public class DisposeArr : DisposeObj
    {
        private IEnumerable<IDisposable> _DisposeList = null;
        public DisposeArr(IEnumerable<IDisposable> disposeList)
        {
            _DisposeList = disposeList;
        }

        protected override void DisposeResources()
        {
            foreach (IDisposable disposeObj in _DisposeList)
            {
                disposeObj.Dispose();
            }
            (_DisposeList as List<IDisposable>).Clear();
            _DisposeList = null;
        }

        //protected override void _Dispose(bool disposing)
        //{
        //    if (!_isDisposed)
        //    {
        //        if (disposing)
        //        {
        //            foreach (IDisposable disposeObj in _DisposeList)
        //            {
        //                disposeObj.Dispose();
        //            }
        //            (_DisposeList as List<IDisposable>).Clear();
        //            _DisposeList = null;
        //        }
        //        //Clean up unmanaged resources;
        //    }
        //    _isDisposed = true;
        //}
    }
}
