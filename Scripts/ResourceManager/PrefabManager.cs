using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviourSingleton<PrefabManager>
{
    [SerializeField] PrefabResourceDB prefabDB;

    public GameObject GetPrefab(int id)
    {
        if (ReferenceEquals(prefabDB, null))
        {
            prefabDB = Resources.Load<PrefabResourceDB>("ResourceDB/PrefabDB");
            if (prefabDB == null)
            {
                DebugLog.LogError("Prefab DB is NULL");
                return null;
            }
        }

        return prefabDB.GetItem(id);
    }
}