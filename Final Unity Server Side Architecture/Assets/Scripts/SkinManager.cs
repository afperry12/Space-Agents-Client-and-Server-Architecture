using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public GameObject defaultSkin;
    private static Dictionary<string, GameObject> _skinPrefabs = new Dictionary<string, GameObject>() {
  { "default", null },
};

    private void Start()
    {
        _skinPrefabs["default"] = defaultSkin;
    }

    public static void AddSkinPrefab(string name, GameObject prefab)
    {
        _skinPrefabs.Add(name, prefab);
    }

    public static GameObject GetSkinPrefab(string name)
    {
        Debug.Log("a");
        if (_skinPrefabs.ContainsKey(name))
        {
            Debug.Log("b");
            return _skinPrefabs[name];
        }
        else
        {
            Debug.Log("d");
            return _skinPrefabs["default"];
        }
    }
}
