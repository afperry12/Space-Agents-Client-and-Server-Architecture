using System;
using KaymakNetwork;
using UnityEngine;

    enum ClientPackets
    {
        CPing = 1,
        CPlayerMovement,
        CPlayerRotation
    }
    internal static class NetworkReceive
    {
        internal static void PacketRouter()
        {
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPing] = Packet_Ping;
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerMovement] = Packet_PlayerMovement;
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerRotation] = Packet_PlayerRotation;
        }

        private static void Packet_Ping(int connectionID, ref byte[] data)
        {
            Debug.Log("Ping Packet Called!");
            ByteBuffer buffer = new ByteBuffer(data);
            string token = buffer.ReadString();
            buffer.Dispose();
            GameManager.instance.CreatePlayer(connectionID, token, "default");
            Console.WriteLine(token);
    }

        private static void Packet_PlayerMovement(int connectionID, ref byte[] data)
        {
            Debug.Log("Player Movement Received");
            ByteBuffer buffer = new ByteBuffer(data);
            bool[] _inputs = new bool[buffer.ReadInt32()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = buffer.ReadBoolean();
            }
            buffer.Dispose();
        
            GameManager.instance.playerList[connectionID].UpdateMovement(connectionID, _inputs);
       }

        private static void Packet_PlayerRotation(int connectionID, ref byte[] data)
        {
            Debug.Log("Player Rotation Received");
            ByteBuffer buffer = new ByteBuffer(data);
            float[] _mouseInputs = new float[buffer.ReadInt32()];
            for (int i = 0; i < _mouseInputs.Length; i++)
            {
                _mouseInputs[i] = buffer.ReadSingle();
            }

            buffer.Dispose();

            // Update the player's orientation based on mouse input
            GameManager.instance.playerList[connectionID].UpdateRotation(_mouseInputs);
        }

}
