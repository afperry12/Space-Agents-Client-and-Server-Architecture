


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

public class GameManager : MonoBehaviour
{
// public GameObject player;
    public int myConnectionID;

    public static GameManager instance;
    
    public Dictionary<int, PlayerManager> playerList = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ItemSpawner> itemSpawners = new Dictionary<int, ItemSpawner>();
    public static Dictionary<int, ProjectileManager> projectiles = new Dictionary<int, ProjectileManager>();
    public static Dictionary<int, EnemyManager> enemies = new Dictionary<int, EnemyManager>();

    public GameObject localPlayer;
    public GameObject onlinePlayer;
    public GameObject itemSpawnerPrefab;
    public GameObject projectilePrefab;
    public GameObject enemyPrefab;
    
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
    
    public void InstantiateNetworkPlayer(int connectionID, string username, Vector3 position, Quaternion rotation, bool isMyPlayer)
    {
        // GameObject go = Instantiate(player);
        // go.name = "Player: " + connectionID;
        //
        // if (isMyPlayer)
        // {
        //     go.AddComponent<InputManager>();
        // }
        //
        // GameManager.instance.playerList.Add(connectionID, go);

        GameObject player;
        if (isMyPlayer)
        {
            player = Instantiate(localPlayer, position, rotation);
        }
        else
        {
            player = Instantiate(onlinePlayer, position, rotation);
        }
        player.GetComponent<PlayerManager>().Initialize(connectionID, username);
        playerList.Add(connectionID, player.GetComponent<PlayerManager>());
        
    }
    public void UninstantiateNetworkPlayer(int connectionID)
    {
        Destroy(GameManager.instance.playerList[connectionID].gameObject);
        GameManager.instance.playerList.Remove(connectionID);
    }
    
    public void CreateItemSpawner(int _spawnerId, Vector3 _position, bool _hasItem)
    {
        GameObject _spawner = Instantiate(itemSpawnerPrefab, _position, itemSpawnerPrefab.transform.rotation);
        _spawner.GetComponent<ItemSpawner>().Initialize(_spawnerId, _hasItem);
        itemSpawners.Add(_spawnerId, _spawner.GetComponent<ItemSpawner>());
    }

    public void SpawnProjectile(int _id, Vector3 _position)
    {
        GameObject _projectile = Instantiate(projectilePrefab, _position, Quaternion.identity);
        _projectile.GetComponent<ProjectileManager>().Initialize(_id);
        projectiles.Add(_id, _projectile.GetComponent<ProjectileManager>());
    }

    public void SpawnEnemy(int _id, Vector3 _position)
    {
        GameObject _enemy = Instantiate(enemyPrefab, _position, Quaternion.identity);
        _enemy.GetComponent<EnemyManager>().Initialize(_id);
        enemies.Add(_id, _enemy.GetComponent<EnemyManager>());
    }

}


//     // public Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
//
//     public static GameManager instance;
//
//     private void Awake()
//     {
//         instance = this;
//     }
//
//     public float WrapEulerAngles(float rotation)
//     {
//         rotation %= 360;
//         if (rotation>160)
//         {
//             return -360;
//         }
//         else
//         {
//             return rotation;
//         }
//     }
//     
//     public float UnwrapEulerAngles(float rotation)
//     {
//         
//         if (rotation>=0)
//         {
//             return rotation;
//         }
//
//         rotation = -rotation % 360;
//
//         return 360 - rotation;
//     }
// }
