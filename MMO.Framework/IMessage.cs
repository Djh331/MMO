using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Framework
{
    /// <summary>
    /// Any message that is sent between servers or clients.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The message type; determines how to handle the message.
        /// </summary>
        MessageType Type { get; }
        /// <summary>
        /// The generic Code of the message to determine (Client: what server to send to) (Server: how to register the message).
        /// Usage (Client): byte cast the ClientOperationCode.
        /// Usage (Server): byte cast the ServerOperationCode.
        /// </summary>
        byte Code { get; }
        /// <summary>
        /// The int casted specific type of message you are sending. May be null for generic Code's with only 1 handler.
        /// Usage (Client and Server): int cast the MessageSubCode.
        /// </summary>
        int? SubCode { get; }
        /// <summary>
        /// Dictionary of parameters to send with the message.
        /// Usage (Client): Each parameter has a byte casted label from ClientParameterCode and the object can be any packaged data.
        /// Usage (Server): Each parameter has a byte casted label from ServerParameterCode and the object can be any packaged data.
        /// </summary>
        Dictionary<byte, object> Parameters { get; }
    }
}
