using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectNameKey
{
    public const string Skill_3 = "";
}

public class ObjectPoolingManager : Singleton<ObjectPoolingManager>
{
    Dictionary<string, Queue<GameObject>> poolingObjectsQueues;

    protected override void Awake()
    {
        base.Awake();
        poolingObjectsQueues = new Dictionary<string, Queue<GameObject>>();
    }

    override public void Init()
    {

    }

    override public void Release()
    {
        poolingObjectsQueues.Clear();
    }

    public void InsertPoolQueue(string key, int maxAount, Transform parent = null)
    {

        if (poolingObjectsQueues.TryGetValue(key, out Queue<GameObject> poolingObjectQueue))
        {
            return;
        }

        poolingObjectsQueues.Add(key, new Queue<GameObject>());
        var que = poolingObjectsQueues[key];
        for (int i = 0; i < maxAount; i++)
        {
            var obj = ObjectManager.Instance.AddObject(key, Vector3.zero, Quaternion.identity, parent);
            obj.SetActive(false);
        }
    }

    public GameObject AddObject(string key, Vector3 position, Quaternion quaternion, bool isLocalPosition = false, bool isLocalRotation = false)
    {
        if (poolingObjectsQueues.TryGetValue(key, out Queue<GameObject> poolingObjectQueue))
        {
            GameObject obj;
            if (poolingObjectQueue.Count == 0)
            {
                for (int i = 0; i < poolingObjectQueue.Count; i++)
                {
                    GameObject val = ObjectManager.Instance.AddObject(key, Vector3.zero, Quaternion.identity);
                    poolingObjectQueue.Enqueue(val);
                    val.SetActive(false);
                }
            }
            obj = poolingObjectQueue.Dequeue();
            obj.SetActive(true);

            if (isLocalPosition == false) obj.transform.position = position;
            else obj.transform.localPosition = position;

            if (isLocalRotation == false) obj.transform.rotation = quaternion;
            else obj.transform.localRotation = quaternion;

            return obj;
        }
        Debug.Log($"{key} is not PollingObjectQueues");
        return null;
    }

    public GameObject AddObject(string key, Vector3 position, Vector3 rot, bool isLocalPosition = false, bool isLocalRotation = false)
    {
        if (poolingObjectsQueues.TryGetValue(key, out Queue<GameObject> poolingObjectQueue))
        {
            var obj = poolingObjectQueue.Dequeue();
            obj.SetActive(false);
            if (isLocalPosition == false) obj.transform.position = position;
            else obj.transform.localPosition = position;

            if (isLocalRotation == false) obj.transform.eulerAngles = rot;
            else obj.transform.localEulerAngles = rot;

            return obj;
        }

        Debug.Log($"{key} is not PollingObjectQueues");
        return null;
    }


    public void DestoryObject(string key, GameObject obj)
    {
        if (poolingObjectsQueues.TryGetValue(key, out Queue<GameObject> poolingObjectQueue))
        {
            poolingObjectQueue.Enqueue(obj);
            obj.SetActive(false);
        }
        Debug.Log($"{key} is not PollingObjectQueues");
    }
}
