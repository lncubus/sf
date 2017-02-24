using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ServiceModel;

using ClientLibrary;

namespace ClientTest
{
    class MainClass
    {
        const int N = 10;

        public static void Main(string[] args)
        {
            using(var listener = new ConsoleTraceListener())
            {
                Debug.Listeners.Add(listener);
                try
                {
                    Run();
                }
                finally
                {
                    Debug.Listeners.Remove(listener);
                }
            }
        }

        private static void Run()
        {
            var binding = new NetTcpBinding(SecurityMode.None, true)
                {
                    //                    MaxBufferPoolSize = 0x100000 Int32.MaxValue,
                    //                    MaxBufferSize = Int32.MaxValue,
                    //                    MaxConnections = Int32.MaxValue,
                    //                    MaxReceivedMessageSize = Int32.MaxValue,
                    CloseTimeout = new TimeSpan(0, 0, 1),
                    OpenTimeout = new TimeSpan(0, 0, 3),
                    ReceiveTimeout = new TimeSpan(0, 0, 2),
                    SendTimeout = new TimeSpan(0, 0, 2),
                    ListenBacklog = 50,
                };
            var address = new EndpointAddress ("net.tcp://localhost:8090");
            var clients = new List<SpaceConnectionClient>();
            var random = new Random();

            int i = 0;
            while (true)
            {
                int n = clients.Count;
                int die = random.Next(N) + random.Next(N);
                if (die >= n)
                {
                    try
                    {
                        var client = new SpaceConnectionClient(binding, address);
                        string nonce = client.Connect();
                        byte[] answer = new byte[16];
                        random.NextBytes(answer);
                        string response = BitConverter.ToString(answer);
                        client.Login(response);
                        clients.Add(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError:\n" + ex.ToString());
                    }
                }
                else if (die < n)
                {
                    try
                    {
                        var client = clients[die];
                        clients.RemoveAt(die);
                        client.Logout();
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError:\n" + ex.ToString());
                    }
                }
                foreach (var client in clients)
                {
                    Console.Write(client.State);
                    Console.Write(" ");
                    if (random.Next(n) >= 2)
                        continue;
                    try
                    {
                        Console.Write(client.Ping());
                        Console.Write(" ");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError:\n" + ex.ToString());
                    }
                }
            }
        }
    }
}
