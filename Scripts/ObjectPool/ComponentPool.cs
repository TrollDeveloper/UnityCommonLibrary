using System.Collections.Generic;
using UnityEngine;

//ObjectPoolManager가 GameObject로 돼있어서, Spawn할때마다 GetComponent 하는 것 방지.
public class ComponentPool<T> where T : Component
{
    private Dictionary<GameObject, T> instanceLookup = new Dictionary<GameObject, T>();

    public T GetComponent(GameObject gameObject)
    {
        if (!instanceLookup.ContainsKey(gameObject))
        {
            instanceLookup.Add(gameObject, gameObject.GetComponent<T>());
        }

        return instanceLookup[gameObject];
    }

    public T AddComponent(GameObject gameObject)
    {
        if (instanceLookup.ContainsKey(gameObject))
        {
            instanceLookup.Remove(gameObject);
        }

        instanceLookup.Add(gameObject, gameObject.AddComponent<T>());

        return instanceLookup[gameObject];
    }

    public void RemoveCachedComponent(GameObject gameObject)
    {
        if (instanceLookup.ContainsKey(gameObject))
        {
            instanceLookup.Remove(gameObject);
        }
    }

    public void RefreshComponent(GameObject gameObject)
    {
        if (instanceLookup.ContainsKey(gameObject))
        {
            instanceLookup.Remove(gameObject);
        }

        instanceLookup.Add(gameObject, gameObject.GetComponent<T>());
    }
}