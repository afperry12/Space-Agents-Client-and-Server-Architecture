using System;
using KaymakNetwork;
using KaymakNetwork.Network;
using UnityEngine;

    enum ServerPackets
    {
        SWelcomeMsg = 1,
        SInstantiatePlayer,
        SUninstantiatePlayer,
        SPlayerPosition,
        SPlayerRotation,
        SInstantiateWorld,
        // SPlayerAnimation,
        SPlayerHealth,
        SPlayerRespawned,
        SCreateItemSpawner,
        SItemSpawned,
        SItemPickedUp,
        SSpawnProjectile,
        SProjectilePosition,
        SProjectileExploded,
        SSpawnEnemy,
        SEnemyPosition,
        SEnemyHealth,
    }
    
    internal static class NetworkReceive
    {
        internal static void PacketRouter()
        {
            NetworkConfig.socket.PacketId[(int) ServerPackets.SWelcomeMsg] = new Client.DataArgs(Packet_WelcomeMsg);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SInstantiatePlayer] = new Client.DataArgs(Packet_InstantiateNetworkPlayer);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SUninstantiatePlayer] = new Client.DataArgs(Packet_UninstantiateNetworkPlayer);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SPlayerPosition] = new Client.DataArgs(Packet_PlayerPosition);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SPlayerRotation] = new Client.DataArgs(Packet_PlayerRotation);
            // NetworkConfig.socket.PacketId[(int) ServerPackets.SPlayerAnimation] = new Client.DataArgs(Packet_PlayerAnimation);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SPlayerHealth] = new Client.DataArgs(Packet_PlayerHealth);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SPlayerRespawned] = new Client.DataArgs(Packet_PlayerRespawned);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SCreateItemSpawner] = new Client.DataArgs(Packet_CreateItemSpawner);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SItemSpawned] = new Client.DataArgs(Packet_ItemSpawned);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SItemPickedUp] = new Client.DataArgs(Packet_ItemPickedUp);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SSpawnProjectile] = new Client.DataArgs(Packet_SpawnProjectile);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SProjectilePosition] = new Client.DataArgs(Packet_ProjectilePosition);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SProjectileExploded] = new Client.DataArgs(Packet_ProjectileExploded);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SSpawnEnemy] = new Client.DataArgs(Packet_SpawnEnemy);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SEnemyPosition] = new Client.DataArgs(Packet_EnemyPosition);
            NetworkConfig.socket.PacketId[(int) ServerPackets.SEnemyHealth] = new Client.DataArgs(Packet_EnemyHealth);
        }


        private static void Packet_WelcomeMsg(ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            int connectionID = buffer.ReadInt32();
            string msg = buffer.ReadString();
            buffer.Dispose();

            GameManager.instance.myConnectionID = connectionID;
            
            string[] args = System.Environment.GetCommandLineArgs ();
            string input = "";
            for (int i = 0; i < args.Length; i++) {
                Debug.Log ("ARG " + i + ": " + args [i]);
                if (args [i] == "--token") {
                    input = args [i + 1];
                    Console.WriteLine(input);
                }
            }
            NetworkSend.SendPing(input);
        }

        private static void Packet_InstantiateNetworkPlayer(ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            int connectionID = buffer.ReadInt32();
            string username = buffer.ReadString();
            float posx = buffer.ReadSingle();
            float posy = buffer.ReadSingle();
            float posz = buffer.ReadSingle();
            float rotx = buffer.ReadSingle();
            float roty = buffer.ReadSingle();
            float rotz = buffer.ReadSingle();
            float rotw = buffer.ReadSingle();

            buffer.Dispose();
            
            Vector3 position = new Vector3(posx, posy, posz);

            Quaternion rotation = new Quaternion(rotx, roty, rotz, rotw);
            Debug.Log(position);
            
        
            if (connectionID == GameManager.instance.myConnectionID)
            {
                GameManager.instance.InstantiateNetworkPlayer(connectionID, username, position, rotation, true);
            }
            else
            {
                GameManager.instance.InstantiateNetworkPlayer(connectionID, username, position, rotation, false);
            }
        }
        
        private static void Packet_UninstantiateNetworkPlayer(ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            int connectionID = buffer.ReadInt32();
            buffer.Dispose();
            
            GameManager.instance.UninstantiateNetworkPlayer(connectionID);
        }
        
        private static void Packet_PlayerPosition(ref byte[] data)
        {
            Debug.Log("Player Pos Received");
            ByteBuffer buffer = new ByteBuffer(data);
            int connectionID = buffer.ReadInt32();
            float x = buffer.ReadSingle();
            float y = buffer.ReadSingle();
            float z = buffer.ReadSingle();
            string animation = buffer.ReadString();
            
            buffer.Dispose();
            
            Debug.Log("Player Position: "+x+" "+y+" "+z);
        
            // if (!GameManager.instance.playerList.ContainsKey(connectionID)) return;
            //
            // GameManager.instance.playerList[connectionID].transform.position = new Vector3(x, y, z);
            if (GameManager.instance.playerList.TryGetValue(connectionID, out PlayerManager _player))
            {
                _player.transform.position = new Vector3(x, y, z);
                _player.currentAnimation = animation;
            }
            Debug.Log("Player Pos Updated");
        }
        
        private static void Packet_PlayerRotation(ref byte[] data)
        {
            Debug.Log("Player Rot Received");
            ByteBuffer buffer = new ByteBuffer(data);
            int connectionID = buffer.ReadInt32();
            float x = buffer.ReadSingle();
            float y = buffer.ReadSingle();
            float z = buffer.ReadSingle();
            float w = buffer.ReadSingle();

            buffer.Dispose();
            float rotationInterpolationSpeed = 10f;

            Quaternion targetRotation = new Quaternion(x, y, z, w);
            if (GameManager.instance.playerList.TryGetValue(connectionID, out PlayerManager _player))
            {
                // Clamp the pitch angle
                // Vector3 euler = targetRotation.eulerAngles;
                // float minPitch = -80f; // The minimum pitch angle
                // float maxPitch = 80f; // The maximum pitch angle
                // euler.x = Mathf.Clamp(euler.x, minPitch, maxPitch);
                // targetRotation = Quaternion.Euler(euler);
                // Interpolate the player's rotation towards the target rotation
                _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, targetRotation, Time.deltaTime * rotationInterpolationSpeed);
            }
            
            // if (GameManager.instance.playerList.TryGetValue(connectionID, out PlayerManager _player))
            // {
            //     _player.model
            //     // Quaternion currentRotation = _player.transform.rotation;
            //     // currentRotation.y += x;
            //     // currentRotation.x -= y;
            //     // _player.transform.rotation = currentRotation;
            //     _player.transform.rotation.Set(x,y,z,w);
            //     
            // }

            
            // if (GameManager.instance.playerList.TryGetValue(connectionID, out PlayerManager _player))
            // {
            //     _player.transform.rotation = rotation;
            // }
            
            // if (connectionID == GameManager.instance.myConnectionID) return;
            //
            // if (!GameManager.instance.playerList.ContainsKey(connectionID)) return;
            // GameManager.instance.playerList[connectionID].transform.rotation = rotation;
        }

        // private static void Packet_PlayerAnimation(ref byte[] data)
        // {
        //     ByteBuffer buffer = new ByteBuffer(data);
        //     int connectionID = buffer.ReadInt32();
        //     int animation = buffer.ReadInt32();
        //     
        //     buffer.Dispose();
        //     
        //     if (!GameManager.instance.playerList.ContainsKey(connectionID)) return;
        //     GameManager.instance.playerList[connectionID].SetAnimation(animation);
        // }
        
        public static void Packet_PlayerHealth(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int connectionID = buffer.ReadInt32();
        float _health = buffer.ReadSingle();

        buffer.Dispose();
        
        GameManager.instance.playerList[connectionID].SetHealth(_health);
    }

    public static void Packet_PlayerRespawned(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int connectionID = buffer.ReadInt32();

        buffer.Dispose();
        
        GameManager.instance.playerList[connectionID].Respawn();
    }

    public static void Packet_CreateItemSpawner(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _spawnerId = buffer.ReadInt32();
        float x = buffer.ReadSingle();
        float y = buffer.ReadSingle();
        float z = buffer.ReadSingle();

        
        bool _hasItem = buffer.ReadBoolean();
        
        buffer.Dispose();
        
        Vector3 _spawnerPosition = new Vector3(x, y, z);
        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _hasItem);
    }

    public static void Packet_ItemSpawned(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _spawnerId = buffer.ReadInt32();
        
        buffer.Dispose();

        GameManager.itemSpawners[_spawnerId].ItemSpawned();
    }

    public static void Packet_ItemPickedUp(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _spawnerId = buffer.ReadInt32();
        int _byPlayer = buffer.ReadInt32();
        
        buffer.Dispose();

        GameManager.itemSpawners[_spawnerId].ItemPickedUp();
        GameManager.instance.playerList[_byPlayer].itemCount++;
    }

    public static void Packet_SpawnProjectile(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _projectileId = buffer.ReadInt32();
        float x = buffer.ReadSingle();
        float y = buffer.ReadSingle();
        float z = buffer.ReadSingle();
        int _thrownByPlayer = buffer.ReadInt32();
        
        buffer.Dispose();
        
        Vector3 _position = new Vector3(x, y, z);
        
        GameManager.instance.SpawnProjectile(_projectileId, _position);
        GameManager.instance.playerList[_thrownByPlayer].itemCount--;
    }

    public static void Packet_ProjectilePosition(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _projectileId = buffer.ReadInt32();
        float x = buffer.ReadSingle();
        float y = buffer.ReadSingle();
        float z = buffer.ReadSingle();
        
        buffer.Dispose();
        
        Vector3 _position = new Vector3(x, y, z);

        if (GameManager.projectiles.TryGetValue(_projectileId, out ProjectileManager _projectile))
        {
            _projectile.transform.position = _position;
        }
    }

    public static void Packet_ProjectileExploded(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _projectileId = buffer.ReadInt32();
        float x = buffer.ReadSingle();
        float y = buffer.ReadSingle();
        float z = buffer.ReadSingle();
        
        buffer.Dispose();
        
        Vector3 _position = new Vector3(x, y, z);

        GameManager.projectiles[_projectileId].Explode(_position);
    }

    public static void Packet_SpawnEnemy(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _enemyId = buffer.ReadInt32();
        float x = buffer.ReadSingle();
        float y = buffer.ReadSingle();
        float z = buffer.ReadSingle();
        
        buffer.Dispose();
        
        Vector3 _position = new Vector3(x, y, z);

        GameManager.instance.SpawnEnemy(_enemyId, _position);
    }

    public static void Packet_EnemyPosition(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _enemyId = buffer.ReadInt32();
        float x = buffer.ReadSingle();
        float y = buffer.ReadSingle();
        float z = buffer.ReadSingle();
        
        buffer.Dispose();
        
        Vector3 _position = new Vector3(x, y, z);

        if (GameManager.enemies.TryGetValue(_enemyId, out EnemyManager _enemy))
        {
            _enemy.transform.position = _position;
        }
    }

    public static void Packet_EnemyHealth(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        int _enemyId = buffer.ReadInt32();
        float _health = buffer.ReadSingle();
        buffer.Dispose();

        GameManager.enemies[_enemyId].SetHealth(_health);
    }
        
    }
