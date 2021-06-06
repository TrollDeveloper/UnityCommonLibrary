using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : class
{
    private List<ObjectPoolContainer<T>> list;
    private Dictionary<T, ObjectPoolContainer<T>> lookup;
    private Func<T> factoryFunc;
    private Action<T> destroyFunc;
    public T Original { get; private set; }
    private int lastIndex = 0;

    public ObjectPool(Func<T> factoryFunc, Action<T> destroyFunc, int initialSize, T origin = null)
    {
        this.factoryFunc = factoryFunc;
        this.destroyFunc = destroyFunc;
        this.Original = origin;

        list = new List<ObjectPoolContainer<T>>(initialSize);
        lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);
        Warm(initialSize);
    }

    public void Warm(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            CreateContainer();
        }
    }

    public void DestoryPool()
    {
        while (list.Count != 0)
        {
            var item = list[0].Item;
            if (lookup.ContainsKey(item))
            {
                lookup.Remove(item);
            }
            destroyFunc?.Invoke(item);

            list.RemoveAt(0);
        }
    }

    private ObjectPoolContainer<T> CreateContainer()
    {
        var container = new ObjectPoolContainer<T>();
        container.Item = factoryFunc();
        list.Add(container);
        return container;
    }

    public T GetItem()
    {
        ObjectPoolContainer<T> container = null;
        for (int i = 0; i < list.Count; i++)
        {
            lastIndex++;
            if (lastIndex > list.Count - 1) lastIndex = 0;

            if (list[lastIndex].Used)
            {
                continue;
            }
            else
            {
                container = list[lastIndex];
                break;
            }
        }

        if (container == null)
        {
            container = CreateContainer();
        }

        container.Consume();
        lookup.Add(container.Item, container);
        return container.Item;
    }

    public void ReleaseItem(object item)
    {
        ReleaseItem((T)item);
    }

    public void ReleaseItem(T item)
    {
        if (lookup.ContainsKey(item))
        {
            var container = lookup[item];
            container.Release();
            lookup.Remove(item);
        }
        else
        {
            Debug.LogWarning("This object pool does not contain the item provided: " + item);
        }
    }

    public int Count
    {
        get { return list.Count; }
    }

    public int CountUsedItems
    {
        get { return lookup.Count; }
    }
}