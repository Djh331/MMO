using MMO.Photon.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO.Photon.Server
{
    public abstract class DefaultResponseHandler : PhotonServerHandler
    {
        protected DefaultResponseHandler(PhotonApplication application)
            : base(application)
        {
        }
    }
}
