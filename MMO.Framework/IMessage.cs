using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    public interface IMessage
    {
        MessageType ype { get; }
        byte code { get; }
        int? SubCode { get; }
        Dictionary<byte, object> Parameters { get; }
    }
}
