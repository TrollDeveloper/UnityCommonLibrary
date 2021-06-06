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
                //�̹� �ִ� �̱���Ŭ���� �޾ƿ���.
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
                    //�̱��� Ŭ���� ������ ������� ����.
                    GameObject gObj = Resources.Load<GameObject>(rootPath + typeof(T).Name);
                    if (gObj != null)
                    {
                        instance = Instantiate(gObj).GetComponent<T>();
                    }

                    //������ ���ο� ������Ʈ�� ����.
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