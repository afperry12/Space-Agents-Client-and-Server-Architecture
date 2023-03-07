using System;
using KaymakNetwork;

namespace NewServer
{
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
            ByteBuffer buffer = new ByteBuffer(data);
            string token = buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine(token);
            GameManager.CreatePlayer(connectionID, token);
        }

        private static void Packet_PlayerMovement(int connectionID, ref byte[] data)
        {
            Console.WriteLine("Player Movement Received");
            ByteBuffer buffer = new ByteBuffer(data);
            bool[] _inputs = new bool[buffer.ReadInt32()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = buffer.ReadBoolean();
            }

            //var x = buffer.ReadSingle();
            //var y = buffer.ReadSingle();
            //var z = buffer.ReadSingle();
            //var w = buffer.ReadSingle();
            buffer.Dispose();

            //Quaternion _rotation = new Quaternion(x, y, z, w);

            GameManager.playerList[connectionID].UpdateMovement(connectionID, _inputs);
            //GameManager.playerList[connectionID].SetInput(connectionID, _inputs, _rotation);
        }

        private static void Packet_PlayerRotation(int connectionID, ref byte[] data)
        {
            Console.WriteLine("Player Rotation Received");
            ByteBuffer buffer = new ByteBuffer(data);
            float[] _mouseInputs = new float[buffer.ReadInt32()];
            for (int i = 0; i < _mouseInputs.Length; i++)
            {
                _mouseInputs[i] = buffer.ReadSingle();
            }

            buffer.Dispose();

            // Update the player's orientation based on mouse input
            GameManager.playerList[connectionID].UpdateRotation(_mouseInputs);
        }


        //public static void PlayerShoot(int connectionID, ref byte[] data)
        //{
        //    ByteBuffer buffer = new ByteBuffer(data);
        //    var x = buffer.ReadSingle();
        //    var y = buffer.ReadSingle();
        //    var z = buffer.ReadSingle();

        //    buffer.Dispose();

        //    Vector3 _shootDirection = new Vector3(x, y, z);

        //    Server.clients[_fromClient].player.Shoot(_shootDirection);
        //}

        //public static void PlayerThrowItem(int connectionID, ref byte[] data)
        //{
        //    ByteBuffer buffer = new ByteBuffer(data);
        //    var x = buffer.ReadSingle();
        //    var y = buffer.ReadSingle();
        //    var z = buffer.ReadSingle();

        //    buffer.Dispose();
        //    Vector3 _throwDirection = new Vector3(x, y, z);

        //    Server.clients[_fromClient].player.ThrowItem(_throwDirection);
        //}

    }
}
