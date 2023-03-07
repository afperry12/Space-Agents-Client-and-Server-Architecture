using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class PlanetManager
{
    //public GameObject defaultPlanet = Resources.Load<GameObject>("Assets/DeepSpaceSkyboxPack/GalacticGreen/Material/Planet 2");
    private static Dictionary<string, GameObject> _planetPrefabs = new Dictionary<string, GameObject>() {
        //{ "default", Resources.Load<GameObject>("Assets/DeepSpaceSkyboxPack/GalacticGreen/Material/Planet 2") }
};

    static PlanetManager()
    {
        _planetPrefabs.Add("default", Resources.Load<GameObject>("Planet 2"));

    }



    public static void AddPlanetPrefab(string name, GameObject prefab)
    {
        _planetPrefabs.Add(name, prefab);
    }

    public static GameObject GetPlanetPrefab(string name)
    {
        if (_planetPrefabs.ContainsKey(name))
        {
            Debug.Log("Here");
            Debug.Log(_planetPrefabs[name]);
            return _planetPrefabs[name];
        }
        else
        {
            Debug.Log("Here1");
            return _planetPrefabs["default"];
        }
    }
}
