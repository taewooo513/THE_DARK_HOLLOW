using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoUI : MonoBehaviour
{
    public string uiKey;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Release()
    {
        UIManager.Instance.RemoveUI(uiKey);
    }
}
