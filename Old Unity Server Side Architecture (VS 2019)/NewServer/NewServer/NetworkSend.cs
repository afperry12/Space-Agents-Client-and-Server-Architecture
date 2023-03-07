using System;
//using UnityEngine;
using System.Numerics;
using KaymakNetwork;

namespace NewServer
{

    enum ServerPackets
    {
        SWelcomeMsg = 1,
        SInstantiatePlayer,
        SUninstantiatePlayer,
        SPlayerPosition,
        SPlayerRotation,
        SPlayerAnimation,
    }

    internal static class NetworkSend
    {
        public static void WelcomeMsg(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SWelcomeMsg);
            buffer.WriteInt32(connectionID);
            buffer.WriteString(msg);
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);

            buffer.Dispose();
        }
        
        private static ByteBuffer PlayerData(int connectionID, Player player)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SInstantiatePlayer);
            buffer.WriteInt32(connectionID);
            buffer.WriteString(GameManager.playerList[connectionID].username);
            buffer.WriteSingle (GameManager.playerList[connectionID].position.X);
            buffer.WriteSingle (GameManager.playerList[connectionID].position.Y);
            buffer.WriteSingle (GameManager.playerList[connectionID].position.Z);
            buffer.WriteSingle (GameManager.playerList[connectionID].rotation.X);
            buffer.WriteSingle (GameManager.playerList[connectionID].rotation.Y);
            buffer.WriteSingle (GameManager.playerList[connectionID].rotation.Z);
            buffer.WriteSingle (GameManager.playerList[connectionID].rotation.W);

            return buffer;
        }

        public static void InstantiateNetworkPlayer(int connectionID, Player player)
        {
            for (int i = 1; i <= GameManager.playerList.Count; i++)
            {
                if (GameManager.playerList[i] != null)
                {
                    //if (GameManager.playerList[i].inGame)
                    //{
                        if (i != connectionID)
                        {
                            NetworkConfig.socket.SendDataTo(connectionID, PlayerData(i, player).Data, PlayerData(i, player).Head);
                        }
                    //}
                }
            }

            NetworkConfig.socket.SendDataToAll(PlayerData(connectionID, player).Data, PlayerData(connectionID, player).Head);
        }

        private static ByteBuffer RemovePlayerData(int connectionID, Player player)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SUninstantiatePlayer);
            buffer.WriteInt32(connectionID);

            return buffer;
        }

        public static void UninstantiateNetworkPlayer(int connectionID, Player player)
        {
            NetworkConfig.socket.SendDataToAll(RemovePlayerData(connectionID, player).Data, RemovePlayerData(connectionID, player).Head);
        }

        //public static void SendPlayerMove(int connectionID, float x, float y, float z)
        //{
        //    ByteBuffer buffer = new ByteBuffer(4);
        //    buffer.WriteInt32((int)ServerPackets.SPlayerMove);
        //    buffer.WriteInt32(connectionID);
        //    buffer.WriteSingle(x);
        //    buffer.WriteSingle(y);
        //    buffer.WriteSingle(z);
        //    NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
        //    buffer.Dispose();
        //}

        //public static void SendPlayerRotation(int connectionID, float rotation)
        //{
        //    ByteBuffer buffer = new ByteBuffer(4);
        //    buffer.WriteInt32((int)ServerPackets.SPlayerRotation);
        //    buffer.WriteInt32(connectionID);
        //    buffer.WriteSingle(rotation);

        //    if (!GameManager.playerList[connectionID].inGame) return;
        //    try
        //    {
        //        NetworkConfig.socket.SendDataToAllBut(connectionID, buffer.Data, buffer.Head);
        //    } catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //    buffer.Dispose();
        //}

        public static void PlayerPosition(int connectionID, Vector3 position)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerPosition);
            buffer.WriteInt32(GameManager.playerList[connectionID].connectionID);
            buffer.WriteSingle(GameManager.playerList[connectionID].position.X);
            buffer.WriteSingle(GameManager.playerList[connectionID].position.Y);
            buffer.WriteSingle(GameManager.playerList[connectionID].position.Z);
            buffer.WriteString(GameManager.playerList[connectionID].animation);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            
            buffer.Dispose();
            Console.WriteLine("Player Movement sent");
        }

        public static void PlayerRotation(int connectionID, Quaternion rotation)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerRotation);
            buffer.WriteInt32(GameManager.playerList[connectionID].connectionID);
            buffer.WriteSingle(rotation.X);
            buffer.WriteSingle(rotation.Y);
            buffer.WriteSingle(rotation.Z);
            buffer.WriteSingle(rotation.W);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }



        //public static void PlayerRotation(int connectionID, Player _player)
        //{
        //    ByteBuffer buffer = new ByteBuffer(4);
        //    buffer.WriteInt32((int)ServerPackets.SPlayerRotation);
        //    buffer.WriteInt32(GameManager.playerList[connectionID].connectionID);
        //    buffer.WriteSingle(GameManager.playerList[connectionID].rotation.X);
        //    buffer.WriteSingle(GameManager.playerList[connectionID].rotation.Y);
        //    buffer.WriteSingle(GameManager.playerList[connectionID].rotation.Z);
        //    buffer.WriteSingle(GameManager.playerList[connectionID].rotation.W);
        //    NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
        //    buffer.Dispose();
        //}

    }
}
