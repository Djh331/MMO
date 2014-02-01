using MMO.Photon.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Photon.Server
{
    public abstract class DefaultRequestHandler : PhotonServerHandler
    {
        public DefaultRequestHandler(PhotonApplication application)
            : base(application)
        {
        }
    }
}
