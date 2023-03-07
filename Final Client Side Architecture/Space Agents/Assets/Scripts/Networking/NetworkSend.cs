using System;
using KaymakNetwork;
using UnityEngine;


enum ClientPackets
    {
        CPing = 1,
        CPlayerMovement,
        CPlayerRotation,
    }
    
    internal static class NetworkSend
    {
        public static void SendPing(String token)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ClientPackets.CPing);
            buffer.WriteString("Received Token: "+token);
            NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
            
            buffer.Dispose();
        }

        // public static void SendKeyInput(InputManager.Keys wPressed, InputManager.Keys aPressed, InputManager.Keys sPressed, InputManager.Keys dPressed)
        // public static void SendKeyInput(InputManager.Keys pressedKey)
        // {
        //     ByteBuffer buffer = new ByteBuffer(4);
        //     buffer.WriteInt32((int)ClientPackets.CKeyInput);
        //     buffer.WriteByte((byte)pressedKey);
        //     NetworkConfig.socket.SendData(buffer.Data,buffer.Head);
        //     
        //     buffer.Dispose();
        // }
    public static void PlayerMovement(bool[] _inputs)
    {
            Debug.Log("Player Pos Started");
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ClientPackets.CPlayerMovement);
            buffer.WriteInt32((int) _inputs.Length);
            foreach (bool _input in _inputs)
            {
                buffer.WriteBoolean(_input);
            }
            // buffer.WriteSingle(GameManager.instance.playerList[GameManager.instance.myConnectionID].transform.rotation.x);
            // buffer.WriteSingle(GameManager.instance.playerList[GameManager.instance.myConnectionID].transform.rotation.y);
            // buffer.WriteSingle(GameManager.instance.playerList[GameManager.instance.myConnectionID].transform.rotation.z);
            // buffer.WriteSingle(GameManager.instance.playerList[GameManager.instance.myConnectionID].transform.rotation.w);
            NetworkConfig.socket.SendData(buffer.Data,buffer.Head);
            
            buffer.Dispose();
            Debug.Log("Player Pos Ended");
        }
    
    public static void PlayerRotation(float[] _mouseInputs)
    {
        Debug.Log("Player Rotation Started");
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int) ClientPackets.CPlayerRotation);
        buffer.WriteInt32((int) _mouseInputs.Length);
        foreach (float _mouseInput in _mouseInputs)
        {
            buffer.WriteSingle(_mouseInput);
        }
        NetworkConfig.socket.SendData(buffer.Data, buffer.Head);
            
        buffer.Dispose();
        Debug.Log("Player Rotation Ended");
    }
    
    // public static void PlayerShoot(Vector3 _facing)
    // {
    //     ByteBuffer buffer = new ByteBuffer(4);
    //     buffer.WriteInt32((int)ClientPackets.CPlayerShoot);
    //     buffer.WriteSingle(_facing.x);
    //     buffer.WriteSingle(_facing.y);
    //     buffer.WriteSingle(_facing.z);
    //     NetworkConfig.socket.SendData(buffer.Data,buffer.Head);
    //     
    //     buffer.Dispose();
    // }
    //
    // public static void PlayerThrowItem(Vector3 _facing)
    // {
    //     ByteBuffer buffer = new ByteBuffer(4);
    //     buffer.WriteInt32((int)ClientPackets.CPlayerThrowItem);
    //     buffer.WriteSingle(_facing.x);
    //     buffer.WriteSingle(_facing.y);
    //     buffer.WriteSingle(_facing.z);
    //     NetworkConfig.socket.SendData(buffer.Data,buffer.Head);
    //     
    //     buffer.Dispose();
    // }
    
}
