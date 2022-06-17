using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_BuildingList : MonoBehaviour
{
    [SerializeField] private string[] keys;
    [SerializeField] private GameObject[] prefabs;

    public GameObject GetPrefabs(string pKey)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == pKey)
            {
                if (prefabs.Length > i)
                    return prefabs[i];
                else
                {
                    Debug.LogError("Building list is not setup");
                    return null;
                }
            }
        }

        return null;
    }
}
