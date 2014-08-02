using System;

namespace MMO.Framework
{
    /// <summary>
    /// Any client is reffered to by his Guid PeerId and contains multiple ClientData.
    /// </summary>
    public interface IClient : IPeer
    {
        /// <summary>
        /// The unique Id that all connected peers have
        /// </summary>
        Guid PeerId { get; set; }
        /// <summary>
        /// A generic holder for ClientData.
        /// Usage: Get current region server, etc.
        /// </summary>
        /// <typeparam name="T">Generic class of Type IClientData to label the data as.</typeparam>
        /// <returns>The ClientData packed data that matches the type of the Generic T.</returns>
        T ClientData<T>() where T : class, IClientData;
    }
}
