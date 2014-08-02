using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    /// <summary>
    /// Interface for Handlers.
    /// </summary>
    /// <typeparam name="T">The type of peer the handler handles from.</typeparam>
    public interface IHandler<T>
    {
        /// <summary>
        /// The type of messgae this handler can handle.
        /// </summary>
        MessageType Type { get; }
        /// <summary>
        /// The byte casted code this handler can handle.
        /// Usage (Client): byte casted ClientOperationCode.
        /// Usage (Server): byte casted ServerOperationCode.
        /// </summary>
        byte Code { get; }
        /// <summary>
        /// The int casted specific type of message this handler can handle. May be null if this is the only handler for this Code.
        /// Usage (Client and Server): int casted MessageSubCode.
        /// </summary>
        int? SubCode { get; }
        /// <summary>
        /// Handles a message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="peer">The peer (Client or Server) that sent the message.</param>
        /// <returns><b>True</b> if the handler successfully handled the message. <b>False</b> otherwise; E.G. an error.</returns>
        bool HandleMessage(IMessage message, T peer);
    }
}
