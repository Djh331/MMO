using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    public interface IHandlerList<T>
    {
        bool RegisteredHandler(IHandler<T> handler);
        bool HandleMessage(IMessage message, T peer);
    }
}
