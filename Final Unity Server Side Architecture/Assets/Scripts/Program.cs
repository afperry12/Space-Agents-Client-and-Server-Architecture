//using UnityEngine;
//using System.Threading;
//using System;

//public class Program : MonoBehaviour
//{
//    private bool isRunning = false;
//    private Thread threadConsole;
//    public static GameObject planetModel;
//    public static Vector3 planetCenter;
//    public static float planetRadius;

//    void Start()
//    {
//        isRunning = true;

//        threadConsole = new Thread(new ThreadStart(consoleThread));
//        threadConsole.Start();

//        //Can add other planets later
//        InstantiatePlanet("default"); // Call method to instantiate planet

//        NetworkConfig.InitNetwork();
//        NetworkConfig.socket.StartListening(5555, 5, 1);
//        Debug.Log("Network has been initialized!");
//    }

//    public static void InstantiatePlanet(string planetPrefabName)
//    {
//        try
//        {
//            // Get the planet prefab using the provided name
//            GameObject planetPrefab = PlanetManager.GetPlanetPrefab(planetPrefabName);
//            if (planetPrefab == null)
//            {
//                Debug.Log("NULLLLLLL!!!");
//            }

//            // Instantiate the planet prefab and store it as a member variable
//            planetModel = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
//            Debug.Log(planetModel);

//            planetModel.AddComponent<SphereCollider>();
//            // Add the GravityAttractor component to the planet
//            //planetModel.AddComponent<GravityAttractor>();

//            // Set the planet's radius as a member variable
//            planetRadius = planetModel.GetComponent<SphereCollider>().radius;

//            // Set the planet's center as a member variable
//            planetCenter = planetModel.transform.position;
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.Message);
//        }
//    }

//    private void consoleThread()
//    {
//        Debug.Log($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
//        DateTime _nextLoop = DateTime.Now;

//        while (isRunning)
//        {
//            while (_nextLoop < DateTime.Now)
//            {
//                // If the time for the next loop is in the past, aka it's time to execute another tick
//                GameLogic.Update(); // Execute game logic

//                _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK); // Calculate at what point in time the next tick should be executed

//                if (_nextLoop > DateTime.Now)
//                {
//                    // If the execution time for the next tick is in the future, aka the server is NOT running behind
//                    Thread.Sleep(_nextLoop - DateTime.Now); // Let the thread sleep until it's needed again.
//                }
//            }
//        }
//    }
//}


using System;
using UnityEngine;

public class Program : MonoBehaviour
{
    public static GameObject planetModel;
    public static Vector3 planetCenter;
    public static float planetRadius;
    public static float planetMass;

    void Start()
    {
        //Dispatcher.Initialize();

        NetworkConfig.InitNetwork();
        NetworkConfig.socket.StartListening(5555, 5, 1);
        Debug.Log("Network has been initialized!");

        Dispatcher.RunOnMainThread(() =>
        {
            // Can add other planets later
            InstantiatePlanet("default"); // Call method to instantiate planet
        });
    }

    public static void InstantiatePlanet(string planetPrefabName)
    {
        try
        {
            // Get the planet prefab using the provided name
            GameObject planetPrefab = PlanetManager.GetPlanetPrefab(planetPrefabName);
            if (planetPrefab == null)
            {
                Debug.Log("NULLLLLLL!!!");
            }

            // Instantiate the planet prefab and store it as a member variable
            planetModel = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log(planetModel);

            planetModel.AddComponent<SphereCollider>();
            // Add the GravityAttractor component to the planet
            //planetModel.AddComponent<GravityAttractor>();

            // Set the planet's radius as a member variable
            planetRadius = planetModel.GetComponent<SphereCollider>().radius;

            // Set the planet's center as a member variable
            planetCenter = planetModel.transform.position;

            planetMass = MassCalculator.CalculateMass(planetModel);
            Debug.Log("Planet mass: " + planetMass);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
