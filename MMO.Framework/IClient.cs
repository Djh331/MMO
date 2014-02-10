using System;

namespace MMO.Framework
{
    public interface IClient : IPeer
    {
        Guid PeerId { get; set; }
        T ClientData<T>() where T : ClientData;
    }
}
