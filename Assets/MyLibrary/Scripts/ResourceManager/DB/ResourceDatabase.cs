using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ResourceDatabase<T> : SerializedScriptableObject where T : class
{
    public Dictionary<int, T> dictionary= new Dictionary<int, T>();

    public virtual T GetItem(int id)
    {
        if (dictionary == null)
        {
            DebugLog.LogError(name + " DataBase is NULL ");
            return null;
        }

        T retVal = null;
        if (dictionary.ContainsKey(id))
        {
            retVal = dictionary[id];
        }
        else
        {
            DebugLog.LogError(name + " DataBase Wrong ID : " + id);
        }
        return retVal;
    }
}
