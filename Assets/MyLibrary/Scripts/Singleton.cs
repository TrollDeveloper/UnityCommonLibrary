using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //이미 있는 싱글톤클래스 받아오기.
                T[] objs = FindObjectsOfType<T>();
                if (objs.Length > 1)
                {
                    DebugLog.LogError("There are Singleotones more than 1" + typeof(T).Name);
                }

                if (objs.Length >= 1)
                {
                    instance = objs[0];
                }
                else
                {
                    //싱글톤 클래스 프리팹 있을경우 생성.
                    GameObject gObj = Resources.Load<GameObject>(rootPath + typeof(T).Name);
                    if (gObj != null)
                    {
                        instance = Instantiate(gObj).GetComponent<T>();
                    }

                    //없으면 새로운 오브젝트로 생성.
                    if (instance == null)
                    {
                        instance = new GameObject().AddComponent<T>();
                        instance.gameObject.name = typeof(T).Name;
                    }
                }
            }
            return instance;
        }
    }
    const string rootPath = "Singleton/";
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}