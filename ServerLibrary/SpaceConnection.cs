using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Threading;

using ClientLibrary;

namespace ServerLibrary
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true,
        UseSynchronizationContext = false
    )]
    public class SpaceConnection : ISpaceConnection
    {
        public const int Version = 0x0030;

        private readonly byte[] challenge;
        private int counter = 0;
        private bool authenticated = false;

        public SpaceConnection()
        {
            Debug.Write(string.Format(
                "{0} : SpaceConnection ", Thread.CurrentThread.ManagedThreadId));
            using (RandomNumberGenerator random = new RNGCryptoServiceProvider())
            {
                challenge = new byte[16];
                random.GetNonZeroBytes(challenge);
            }
            Debug.WriteLine(string.Format(
                " challenge = {0}", BitConverter.ToString(challenge)));
        }

        public string Connect()
        {
            Debug.Write(string.Format(
                "{0} : Connect ", Thread.CurrentThread.ManagedThreadId));
            string nonce = Convert.ToBase64String(challenge);
            Debug.WriteLine(string.Format(
                "nonce -> {0}", nonce));
            return nonce;
        }

        public bool Login(string response)
        {
            authenticated = true;
            Debug.WriteLine(string.Format(
                "{0} : Login response <- {1} authenticated -> {2}",
                Thread.CurrentThread.ManagedThreadId,
                response,
                authenticated
            ));
            return authenticated;
        }

        public void Logout()
        {
            authenticated = false;
            Debug.WriteLine(string.Format(
                "{0} : Logout",
                Thread.CurrentThread.ManagedThreadId
            ));
        }

        public int Ping()
        {
            counter++;
            Debug.WriteLine(string.Format(
                "{0} : Ping {1}",
                Thread.CurrentThread.ManagedThreadId,
                counter
            ));
            return counter;
        }
    }
}

