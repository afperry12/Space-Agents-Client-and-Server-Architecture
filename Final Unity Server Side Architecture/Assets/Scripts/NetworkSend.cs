using System;
using UnityEngine;
using KaymakNetwork;


    enum ServerPackets
    {
        SWelcomeMsg = 1,
        SInstantiatePlayer,
        SUninstantiatePlayer,
        SPlayerPosition,
        SPlayerRotation,
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
            buffer.WriteString(GameManager.instance.playerList[connectionID].username);
            buffer.WriteSingle (GameManager.instance.playerList[connectionID].position.x);
            buffer.WriteSingle (GameManager.instance.playerList[connectionID].position.y);
            buffer.WriteSingle (GameManager.instance.playerList[connectionID].position.z);
            buffer.WriteSingle (GameManager.instance.playerList[connectionID].rotation.x);
            buffer.WriteSingle (GameManager.instance.playerList[connectionID].rotation.y);
            buffer.WriteSingle (GameManager.instance.playerList[connectionID].rotation.z);
            buffer.WriteSingle (GameManager.instance.playerList[connectionID].rotation.w);

            return buffer;
        }

        public static void InstantiateNetworkPlayer(int connectionID, Player player)
        {
            for (int i = 1; i <= GameManager.instance.playerList.Count; i++)
            {
                if (GameManager.instance.playerList[i] != null)
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

    //public static void PlayerPosition(int connectionID)
    //{
    //    ByteBuffer buffer = new ByteBuffer(4);
    //    buffer.WriteInt32((int)ServerPackets.SPlayerPosition);
    //    buffer.WriteInt32(GameManager.instance.playerList[connectionID].connectionID);
    //buffer.WriteSingle(GameManager.instance.playerList[connectionID].playerModel.transform.position.x);
    //buffer.WriteSingle(GameManager.instance.playerList[connectionID].playerModel.transform.position.y);
    //buffer.WriteSingle(GameManager.instance.playerList[connectionID].playerModel.transform.position.z);
    ////buffer.WriteSingle(position.x);
    ////buffer.WriteSingle(position.y);
    ////buffer.WriteSingle(position.z);
    //buffer.WriteString(GameManager.instance.playerList[connectionID].animation);
    //    NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
    //    buffer.Dispose();
    //    Debug.Log("Player Movement sent");
    //}

    public static void PlayerPosition(int connectionID, GameObject playerModel)
    {
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int)ServerPackets.SPlayerPosition);
        buffer.WriteInt32(GameManager.instance.playerList[connectionID].connectionID);
        buffer.WriteSingle(playerModel.transform.position.x);
        buffer.WriteSingle(playerModel.transform.position.y);
        buffer.WriteSingle(playerModel.transform.position.z);
        buffer.WriteString(GameManager.instance.playerList[connectionID].animation);
        NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);

        buffer.Dispose();
        Console.WriteLine("Player Movement sent");
    }

    public static void PlayerRotation(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerRotation);
            buffer.WriteInt32(GameManager.instance.playerList[connectionID].connectionID);
        buffer.WriteSingle(GameManager.instance.playerList[connectionID].playerModel.transform.rotation.x);
        buffer.WriteSingle(GameManager.instance.playerList[connectionID].playerModel.transform.rotation.y);
        buffer.WriteSingle(GameManager.instance.playerList[connectionID].playerModel.transform.rotation.z);
        buffer.WriteSingle(GameManager.instance.playerList[connectionID].playerModel.transform.rotation.w);
        //buffer.WriteSingle(rotation.x);
        //buffer.WriteSingle(rotation.y);
        //buffer.WriteSingle(rotation.z);
        //buffer.WriteSingle(rotation.w);
        NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
            Debug.Log("Player Rotation sent");
    }

}
