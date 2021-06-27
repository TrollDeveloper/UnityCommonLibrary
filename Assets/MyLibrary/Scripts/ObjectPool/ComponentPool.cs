using System.Collections.Generic;
using UnityEngine;

//ObjectPoolManager가 GameObject로 돼있어서, Spawn할때마다 GetComponent 하는 것 방지.
public class ComponentPool<T> where T : MonoBehaviour
{
    private Dictionary<GameObject, T> instanceLookup = new Dictionary<GameObject, T>();

    public T GetComponent(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return null;
        }

        if (instanceLookup.ContainsKey(gameObject) == false)
        {
            instanceLookup.Add(gameObject, gameObject.GetComponent<T>());
        }

        return instanceLookup[gameObject];
    }
}