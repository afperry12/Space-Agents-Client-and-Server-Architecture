//using Newtonsoft.Json;
//using SocketIOClient;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using UnityEngine;
//using UnityEngine.UIElements;


//public class GameManager : MonoBehaviour
//{
//        public static Dictionary<int, SocketIO> socketList = new Dictionary<int, SocketIO>();
//        //public static SocketIO client = new SocketIO("http://localhost:8080/");
//        public static Dictionary<int, Player> playerList = new Dictionary<int, Player>();
//        public static float playerSpeed = 0.1f;
//        public static GameObject planetModel;
//        public static Vector3 planetCenter;
//        public static float planetRadius;
//    public static GameObject playerModel;
//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else if (instance != this)
//        {
//            Debug.Log("Instance already exists, destroying object!");
//            Destroy(this);
//        }
//    }

//        public static void JoinGame(int connectionID, Player player, string skinPrefabName)
//        {
//            Debug.Log("Join Game Called!");
//        InstantiatePlayer(skinPrefabName);

//        NetworkSend.InstantiateNetworkPlayer(connectionID, player);
//        SendPlayerSocketAsync(connectionID, player);
//            Debug.Log("Join Game FINISHED!");
//    }


//    public static void LeaveGame(int connectionID, Player player)
//        {
//            Debug.Log("Leave Game Called!");
//            NetworkSend.UninstantiateNetworkPlayer(connectionID, player);
//            ClosePlayerSocketAsync(GameManager.socketList[connectionID]);
//            GameManager.socketList.Remove(connectionID);
//        }

//    public static async void CreatePlayer(int connectionID, string token)
//        {
//            var client = new HttpClient();
//            var httpRequestMessage = new HttpRequestMessage
//            {
//                Method = HttpMethod.Get,
//                RequestUri = new Uri("http://localhost:3002/isUserAuth"),
//                Headers = {
//            { "x-access-token", token }
//        },
//            };

//            var response = client.SendAsync(httpRequestMessage).Result;

//            var bodyResponseString = await response.Content.ReadAsStringAsync();

//            dynamic obj = JsonConvert.DeserializeObject<dynamic>(bodyResponseString);

//            Console.WriteLine(bodyResponseString);

//            // Will need to use at some point
//            // string _username = obj.user;
//            Player player = new Player(connectionID, "_username", new Vector3(0, 0, 0), "default");
//            playerList.Add(connectionID, player);
//            Debug.Log("Player " + connectionID+" has been added to the game");
//            //Will need to update later to other than default
//            JoinGame(connectionID, player, "default");
//        }

//            public static void RemovePlayer(int connectionID)
//        {
//            Player player = playerList[connectionID];


//            playerList.Remove(connectionID);
//            LeaveGame(connectionID, player);
//        Debug.Log("Player " + connectionID + " has quit the game");
//    }

//        public static async void SendPlayerSocketAsync(int connectionID, Player player)
//        {
//            SocketIO client = new SocketIO("http://localhost:8080/");
//            socketList.Add(connectionID, client);
//            try
//            {
//                Console.WriteLine("here1");
//                client.On("hi", response =>
//                {
//                    Console.WriteLine("here2");
//                    string text = response.GetValue<string>();
//                    Console.WriteLine(text);
//                });
//                Console.WriteLine("here3");
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("heree1");
//                Console.WriteLine(e);
//            }

//            try
//            {
//                Console.WriteLine("here4");
//                client.OnConnected += async (sender, e) =>
//                {
//                    Console.WriteLine("here5");
//                    await client.EmitAsync("hi", "playername");
//                    Console.WriteLine("here6");
//                };
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("heree2");
//                Console.WriteLine(e);
//            }
//            try
//            {
//                Console.WriteLine("here8");
//                await client.ConnectAsync();
//                Console.WriteLine("here9");
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("heree3");
//                Console.WriteLine(e);
//            }

//        }

//        public static async void ClosePlayerSocketAsync(SocketIO client)
//        {
//            try
//            {
//                await client.EmitAsync("bye", "playername");
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
//            try
//            {
//                Console.WriteLine("here10");
//                client.OnDisconnected += async (sender, e) =>
//                {
//                    client.Off("hi");
//                    Console.WriteLine("here11");
//                    //await client.EmitAsync("hi", "playername");
//                    Console.WriteLine("here12");
//                };
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("heree4");
//                Console.WriteLine(e);
//            }
//            try
//            {
//                Console.WriteLine("here13");
//                await client.DisconnectAsync();
//                //await client.Socket.DisconnectAsync();
//                Console.WriteLine("here14");
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("heree5");
//                Console.WriteLine(e);
//            }
//        }

//    public static void InstantiatePlanet(string planetPrefabName)
//    {
//        // Get the planet prefab using the provided name
//        GameObject planetPrefab = PlanetManager.GetPlanetPrefab(planetPrefabName);
//        if (planetPrefab == null) {
//        Debug.Log("NULLLLLLL!!!");
//        }

//        // Instantiate the planet prefab and store it as a member variable
//        planetModel = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);

//        planetModel.AddComponent<SphereCollider>();
//        // Add the GravityAttractor component to the planet
//        //planetModel.AddComponent<GravityAttractor>();

//        // Set the planet's radius as a member variable
//        planetRadius = planetModel.GetComponent<SphereCollider>().radius;

//        // Set the planet's center as a member variable
//        planetCenter = planetModel.transform.position;
//    }

//    public static void InstantiatePlayer(string playerPrefabName)
//    {

//        GameObject player;
//        //Get the planet prefab using the provided name
//           GameObject playerPrefab = SkinManager.GetSkinPrefab(playerPrefabName);
//        if (playerPrefab == null)
//        {
//            Debug.Log("NULLLLLLL!!!");
//        }
//        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
//        //player.GetComponent<Player>().Initialize(connectionID);
//        //// When creating the player object, queue an action to be executed on the main thread to instantiate the player model.
//        //ThreadManager.ExecuteOnMainThread(() =>
//        //{
//        //    // Get the planet prefab using the provided name
//        //    GameObject playerPrefab = SkinManager.GetSkinPrefab(playerPrefabName);
//        //    if (playerPrefab == null)
//        //    {
//        //        Debug.Log("NULLLLLLL!!!");
//        //    }

//        //    try
//        //    {
//        //        // Instantiate the planet prefab and store it as a member variable
//        //        playerModel = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        Debug.Log(e.Message);
//        //    }
//        //});


//    }


//}

using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEditor.MemoryProfiler;
using UnityEditor.VersionControl;
using UnityEngine;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Dictionary<int, SocketIO> socketList = new Dictionary<int, SocketIO>();
    //public static SocketIO client = new SocketIO("http://localhost:8080/");
    public Dictionary<int, Player> playerList = new Dictionary<int, Player>();
    public float playerSpeed = 0.1f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void CreatePlayer(int connectionID, string token, string skinPrefabName)
    {
        Debug.Log("In create player start");
            //Get the planet prefab using the provided name
            GameObject playerPrefab = SkinManager.GetSkinPrefab(skinPrefabName);
            if (playerPrefab == null)
            {
                Debug.Log("NULLLLLLL!!!");
            }
            try
            {
                GameObject player;
            Dispatcher.RunOnMainThread(() =>
            {
                player = Instantiate(playerPrefab, new Vector3(0, 1000, 0), Quaternion.identity);
                player.GetComponent<Player>().Initialize(connectionID, "_username", new Vector3(0, 1000, 0), player);
                instance.playerList.Add(connectionID, player.GetComponent<Player>());
                NetworkSend.InstantiateNetworkPlayer(connectionID, player.GetComponent<Player>());
                instance.SendPlayerSocketAsync(connectionID, player.GetComponent<Player>());
            });
            Debug.Log("Created Player Successfully!");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        Debug.Log("Out create player start");
    }
    public void RemovePlayer(int connectionID)
    {
        // Check if the player with the given connection ID exists
        if (playerList.ContainsKey(connectionID))
        {
            Player player = GameManager.instance.playerList[connectionID];

            // Destroy the gameobject associated with the player
            //Destroy(player.gameObject);

            playerList.Remove(connectionID);
            Debug.Log("REMOVE PLAYER CALLED");
            Dispatcher.RunOnMainThread(() =>
            {
                Destroy(player.playerModel);
            });
            //Debug.Log("Leave Game Called!");
            NetworkSend.UninstantiateNetworkPlayer(connectionID, player);
            ClosePlayerSocketAsync(socketList[connectionID]);
            GameManager.instance.socketList.Remove(connectionID);
            Debug.Log("Player " + connectionID + " has quit the game");
        } else
        {
            Debug.Log("Connection ID does not exist in player list");
        }
    }



    //public async void CreatePlayer(int connectionID, string token, string skinPrefabName)
    //{
    //    var client = new HttpClient();
    //    var httpRequestMessage = new HttpRequestMessage
    //    {
    //        Method = HttpMethod.Get,
    //        RequestUri = new Uri("http://localhost:3002/isUserAuth"),
    //        Headers = {
    //        { "x-access-token", token }
    //    },
    //    };

    //    var response = client.SendAsync(httpRequestMessage).Result;

    //    var bodyResponseString = await response.Content.ReadAsStringAsync();

    //    dynamic obj = JsonConvert.DeserializeObject<dynamic>(bodyResponseString);

    //    Console.WriteLine(bodyResponseString);

    //    // Will need to use at some point
    //    // string _username = obj.user;
    //already done:
    //    Player player = new Player(connectionID, "_username", new Vector3(0, 0, 0), "default");
    //    playerList.Add(connectionID, player);
    //    Debug.Log("Player " + connectionID + " has been added to the game");
    //    Debug.Log("Join Game Called!");
    //    InstantiatePlayer(skinPrefabName);

    //    NetworkSend.InstantiateNetworkPlayer(connectionID, player);
    //    SendPlayerSocketAsync(connectionID, player);
    //    Debug.Log("Join Game FINISHED!");
    //}


    public async void SendPlayerSocketAsync(int connectionID, Player player)
    {
        SocketIO client = new SocketIO("http://localhost:8080/");
        instance.socketList.Add(connectionID, client);
        try
        {
            client.On("hi", response =>
            {
                string text = response.GetValue<string>();
                Console.WriteLine(text);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        try
        {
            client.OnConnected += async (sender, e) =>
            {
                await client.EmitAsync("hi", "playername");
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        try
        {
            await client.ConnectAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

    public async void ClosePlayerSocketAsync(SocketIO client)
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
            client.OnDisconnected += async (sender, e) =>
            {
                client.Off("hi");
                //await client.EmitAsync("hi", "playername");
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        try
        {
            await client.DisconnectAsync();
            //await client.Socket.DisconnectAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }



}