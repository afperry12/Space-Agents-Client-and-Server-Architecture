using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using SocketIOClient;

namespace NewServer
{
    class Program
    {
        private static bool isRunning = false;
        private static Thread threadConsole;

        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            isRunning = true;

            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();
            
            NetworkConfig.InitNetwork();
            NetworkConfig.socket.StartListening(5555, 5, 1);
            Console.WriteLine("Network has been initialized!");


            //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //IPEndPoint ipEnd = new IPEndPoint(ipAddress, 3004);
            //Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            //TcpClient client = new TcpClient();
            //client.Connect("localhost", 3000); //Connect to the server on our local host IP address, listening to port 3000
            //NetworkStream clientStream = client.GetStream();

        }

        private static void ConsoleThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    // If the time for the next loop is in the past, aka it's time to execute another tick
                    GameLogic.Update(); // Execute game logic

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK); // Calculate at what point in time the next tick should be executed

                    if (_nextLoop > DateTime.Now)
                    {
                        // If the execution time for the next tick is in the future, aka the server is NOT running behind
                        Thread.Sleep(_nextLoop - DateTime.Now); // Let the thread sleep until it's needed again.
                    }
                }
            }
        }

    }
}
