using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviourSingleton<ObjectPoolManager>
{
    private Dictionary<GameObject, ObjectPool<GameObject>> prefabLookup;
    private Dictionary<GameObject, ObjectPool<GameObject>> instanceLookup;
    private Dictionary<GameObject, Transform> parentLookup;

    protected override void Awake()
    {
        base.Awake();
        prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        parentLookup = new Dictionary<GameObject, Transform>();
    }

    public void WarmPool(GameObject prefab, int size)
    {
        ObjectPool<GameObject> pool;
        if (prefabLookup.ContainsKey(prefab))
        {
            pool = prefabLookup[prefab];
            if (pool.Count < size)
            {
                pool.Warm(pool.Count - size);
            }

            DebugLog.Log("Pool for prefab " + prefab.name + " has already been created");
        }
        else
        {
            pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(prefab); }, DestroyObject, size, prefab);
        }

        prefabLookup[prefab] = pool;
    }

    public void DestroyPool(GameObject prefab)
    {
        if (prefabLookup.ContainsKey(prefab))
        {
            var pool = prefabLookup[prefab];
            pool.DestoryPool();
            prefabLookup.Remove(prefab);
        }

        //부모오브젝트 제거.
        if (parentLookup.ContainsKey(prefab))
        {
            Destroy(parentLookup[prefab]);
            parentLookup.Remove(prefab);
        }
    }

    public GameObject SpawnObject(GameObject prefab)
    {
        return SpawnObject(prefab, Vector3.zero, Quaternion.identity);
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!prefabLookup.ContainsKey(prefab))
        {
            WarmPool(prefab, 1);
        }

        var pool = prefabLookup[prefab];

        var clone = pool.GetItem();
        clone.transform.SetPositionAndRotation(position, rotation);
        clone.SetActive(true);

        instanceLookup.Add(clone, pool);
        return clone;
    }

    public T SpawnObject<T>(GameObject prefab, ComponentPool<T> componentPool) where T : MonoBehaviour
    {
        return SpawnObject<T>(prefab, componentPool, Vector3.zero, Quaternion.identity);
    }

    public T SpawnObject<T>(GameObject prefab, ComponentPool<T> componentPool, Vector3 position,
        Quaternion rotation) where T : MonoBehaviour
    {
        return componentPool.GetComponent(SpawnObject(prefab, position, rotation));
    }

    public void ReleaseObject(GameObject clone)
    {
        clone.SetActive(false);

        if (instanceLookup.ContainsKey(clone))
        {
            var pool = instanceLookup[clone];

            //반환시 오브젝트 풀 부모로 재설정.
            if (parentLookup.ContainsKey(pool.Original))
            {
                clone.transform.parent = parentLookup[pool.Original];
            }

            //스케일 원본으로 변경.
            if (pool.Original != null)
            {
                clone.transform.localScale = pool.Original.transform.localScale;
            }

            pool.ReleaseItem(clone);

            instanceLookup.Remove(clone);
        }
        else
        {
            Debug.LogWarning("No pool contains the object: " + clone.name);
        }
    }


    private GameObject InstantiatePrefab(GameObject prefab)
    {
        var go = Instantiate(prefab) as GameObject;

        //부모 찾기.
        Transform parent = transform;
        if (parentLookup.ContainsKey(prefab))
        {
            parent = parentLookup[prefab];
        }
        else
        {
            //부모없으면 새로생성.
            parent = new GameObject().transform;
            parent.parent = transform;
            parent.name = prefab.name;
            parentLookup.Add(prefab, parent);
        }

        //부모설정 후 Deactive.
        go.transform.parent = parent;
        go.SetActive(false);
        return go;
    }

    private void DestroyObject(GameObject clone)
    {
        if (instanceLookup.ContainsKey(clone))
        {
            instanceLookup.Remove(clone);
        }

        Destroy(clone);
    }
}