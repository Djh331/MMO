using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    public interface IBackgroundThread
    {
        void Setup();
        void Run(Object threadContext);
        void Stop();
    }
}
