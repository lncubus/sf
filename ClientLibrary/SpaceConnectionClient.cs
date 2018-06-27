using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ClientLibrary
{
    public class SpaceConnectionClient : ClientBase<ISpaceConnection>, ISpaceConnection
    {
        public SpaceConnectionClient (Binding binding, EndpointAddress address) : base (binding, address)
        {
        }

        public string Connect ()
        {
            Debug.WriteLine("Connect");
            return Channel.Connect();
        }

        public bool Login(string response)
        {
            Debug.WriteLine("Login");
            return Channel.Login(response);
        }

        public void Logout()
        {
            Debug.WriteLine("Logout");
            Channel.Logout();
        }

        public int Ping()
        {
            return Channel.Ping();
        }
    }
}

