using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ClientLibrary
{
//    public class SpaceClient: IDisposable
//    {
//        public SpaceClient()
//        {
//        }
//
//        public void Dispose()
//        {
//        }
//    }

    public class SpaceConnectionClient : ClientBase<ISpaceConnection>, ISpaceConnection
    {
        public SpaceConnectionClient (Binding binding, EndpointAddress address) : base (binding, address)
        {
        }

        public string Connect ()
        {
            Debug.Write("(Connect...");
            var result = Channel.Connect();
            Debug.WriteLine("...)");
            return result;
        }

        public bool Login(string response)
        {
 //           Debug.Write("(Login...");
            var result = Channel.Login(response);
   //         Debug.WriteLine("...)");
            return result;
        }

        public void Logout()
        {
     //       Debug.Write("(Logout...");
            Channel.Logout();
       //     Debug.WriteLine("...)");
        }

        public int Ping()
        {
         //   Debug.Write("(Ping...");
            var result = Channel.Ping();
           // Debug.WriteLine("...)");
            return result;
        }
    }
}

