using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static public T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            if (transform.parent == null)
                DontDestroyOnLoad(gameObject);
            instance = GetComponent<T>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Init()
    {

    }

    protected virtual void Release()
    {

    }
}
