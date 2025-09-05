using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, MonoUI> uiManagers;

    protected override void Awake()
    {
        base.Awake();
        uiManagers = new Dictionary<string, MonoUI>();
    }

    //매니저를 유아이 캔버스쪽에다 넣어두고 사용
    public void AddUIManager(string key, MonoUI monoUI)
    {
        if (uiManagers.TryGetValue(key, out MonoUI res))
        {
            Debug.Log($"{key} is duplicate in UIManager");
            return;
        }
        uiManagers.Add(key, monoUI);
    }


    public T FindUIManager<T>(string key) where T : MonoUI
    {
        if (uiManagers.TryGetValue(key, out MonoUI res))
        {
            return (T)res;
        }
        Debug.Log($"{key} is not find UIManager");
        return null;
    }

    public void SetActive(string key, bool isActive)
    {
        if (uiManagers.TryGetValue(key, out MonoUI res))
        {
            res.SetActive(isActive);
        }
        Debug.Log($"{key} is not find UIManager");
    }

    public void RemoveUI(string key)
    {
        if (uiManagers.TryGetValue(key, out MonoUI res))
        {
            uiManagers.Remove(key);
        }
    }
    public void ClearUIManager()
    {
        uiManagers.Clear();
    }
}
