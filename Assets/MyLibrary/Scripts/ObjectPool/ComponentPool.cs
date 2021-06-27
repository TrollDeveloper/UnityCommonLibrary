using System.Collections.Generic;
using UnityEngine;

//ObjectPoolManager�� GameObject�� ���־, Spawn�Ҷ����� GetComponent �ϴ� �� ����.
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