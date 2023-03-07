using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
//using UnityEngine;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NewServer
{
    static class GameManager
    {
        public static Dictionary<int, SocketIO> socketList = new Dictionary<int, SocketIO>();
        //public static SocketIO client = new SocketIO("http://localhost:8080/");
        public static Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        public static float playerSpeed = 0.1f;

        public static void JoinGame(int connectionID, Player player)
        {
            NetworkSend.InstantiateNetworkPlayer(connectionID, player);
            SendPlayerSocketAsync(connectionID, player);
        }
        
        public static void LeaveGame(int connectionID, Player player)
        {
            Console.WriteLine("Leave Game Called!");
            NetworkSend.UninstantiateNetworkPlayer(connectionID, player);
            ClosePlayerSocketAsync(GameManager.socketList[connectionID]);
            GameManager.socketList.Remove(connectionID);
        }

        public static async void CreatePlayer(int connectionID, string token)
        {
            var client = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:3002/isUserAuth"),
                Headers = {
            { "x-access-token", token }
        },
            };

            var response = client.SendAsync(httpRequestMessage).Result;

            var bodyResponseString = await response.Content.ReadAsStringAsync();

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(bodyResponseString);

            //Console.WriteLine(obj.user);

            Console.WriteLine(bodyResponseString);


            string _username = obj.user;
            Player player = new Player(connectionID, _username, new Vector3(0, 5000, 0));
            //{
            //    connectionID = connectionID,
            //    inGame = true,
            //    Token = token,
            //};
            playerList.Add(connectionID, player);
            Console.WriteLine("Player '{0}' has been added to the game", connectionID);
            JoinGame(connectionID, player);
        }

            public static void RemovePlayer(int connectionID)
        {
            Player player = playerList[connectionID];
            

            playerList.Remove(connectionID);
            LeaveGame(connectionID, player);
            Console.WriteLine("Player '{0}' has quit the game", connectionID);
        }

        public static async void SendPlayerSocketAsync(int connectionID, Player player)
        {
            SocketIO client = new SocketIO("http://localhost:8080/");
            socketList.Add(connectionID, client);
            //client.On("hi", response =>
            //{
            //    string text = response.GetValue<string>();
            //    Console.WriteLine(text);
            //});
            try
            {
                Console.WriteLine("here1");
                client.On("hi", response =>
                {
                    Console.WriteLine("here2");
                    string text = response.GetValue<string>();
                    Console.WriteLine(text);
                });
                Console.WriteLine("here3");
            }
            catch (Exception e)
            {
                Console.WriteLine("heree1");
                Console.WriteLine(e);
            }

            try
            {
                Console.WriteLine("here4");
                client.OnConnected += async (sender, e) =>
                {
                    Console.WriteLine("here5");
                    await client.EmitAsync("hi", "playername");
                    Console.WriteLine("here6");
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("heree2");
                Console.WriteLine(e);
            }
            try
            {
                Console.WriteLine("here8");
                await client.ConnectAsync();
                Console.WriteLine("here9");
            }
            catch (Exception e)
            {
                Console.WriteLine("heree3");
                Console.WriteLine(e);
            }



            //try
            //{
            //    Console.WriteLine("here10");
            //    client.OnDisconnected -= async (sender, e) =>
            //    {
            //        client.OffAny();
            //        Console.WriteLine("here11");
            //        //await client.EmitAsync("hi", "playername");
            //        Console.WriteLine("here12");
            //    };
            //} catch (Exception e)
            //{
            //    Console.WriteLine("heree4");
            //    Console.WriteLine(e);
            //}
            //try
            //{
            //    Console.WriteLine("here13");
            //    await client.DisconnectAsync();
            //    Console.WriteLine("here14");
            //} catch (Exception e)
            //{
            //    Console.WriteLine("heree5");
            //    Console.WriteLine(e);
            //}




            //TcpClient client = new TcpClient();
            //client.Connect("localhost", 8080); //Connect to the server on our local host IP address, listening to port 3000
            //NetworkStream clientStream = client.GetStream();
            //System.Threading.Thread.Sleep(1000); //Sleep before we get the data for 1 second
            //while (clientStream.DataAvailable)
            //{
            //    byte[] inMessage = new byte[4096];
            //    int bytesRead = 0;
            //    try
            //    {
            //        bytesRead = clientStream.Read(inMessage, 0, 4096);
            //    }
            //    catch { /*Catch exceptions and handle them here*/ }

            //    ASCIIEncoding encoder = new ASCIIEncoding();
            //    Console.WriteLine(encoder.GetString(inMessage, 0, bytesRead));
            //}
            ////******************** SEND DATA **********************************
            //string message = connectionID.ToString() + player.ToString();
            //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            //// Call StoreAsync method to store the data to a backing stream
            //NetworkStream stream = client.GetStream();
            //stream.Write(data, 0, data.Length);
            //Console.WriteLine("Sent: {0}", message);
            //// Buffer to store the response bytes.
            //data = new Byte[256];

            //// String to store the response ASCII representation.
            //String responseData = String.Empty;

            //// Read the first batch of the TcpServer response bytes.
            //Int32 bytes = stream.Read(data, 0, data.Length);
            //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            //Console.WriteLine("Received: {0}", responseData);

            //// Close everything.
            //stream.Close();
            ////*****************************************************************
            //client.Close();
            ////System.Threading.Thread.Sleep(10000); //Sleep for 10 seconds
        }

        public static async void ClosePlayerSocketAsync(SocketIO client)
        {
            try
            {
                await client.EmitAsync("bye", "playername");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                Console.WriteLine("here10");
                client.OnDisconnected += async (sender, e) =>
                {
                    client.Off("hi");
                    Console.WriteLine("here11");
                    //await client.EmitAsync("hi", "playername");
                    Console.WriteLine("here12");
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("heree4");
                Console.WriteLine(e);
            }
            try
            {
                Console.WriteLine("here13");
                await client.DisconnectAsync();
                await client.Socket.DisconnectAsync();
                Console.WriteLine("here14");
            }
            catch (Exception e)
            {
                Console.WriteLine("heree5");
                Console.WriteLine(e);
            }
        }

    }
}
