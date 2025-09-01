using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    Dictionary<string, GameObject> objects;
    void Awake()
    {
        objects = new Dictionary<string, GameObject>();
    }

    public override void Release()
    {
        objects.Clear();
    }

    public void InsertObject(string key, GameObject obj)
    {
        if (objects.TryGetValue(key, out GameObject res))
        {
            Debug.Log($"{key} is duplicate in obj");
            return;
        }
        objects.Add(key, obj);
    }

    public GameObject AddObject(string key, Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        if (objects.TryGetValue(key, out GameObject res))
        {
            if (parent == null)
            {
                return Instantiate(res, position, quaternion);
            }
            else
            {
                return Instantiate(res, position, quaternion, parent);
            }
        }
        Debug.Log($"Not Find {key} in Objects");
        return null;
    }
}
