using UnityEngine;

public class DDOL : MonoBehaviour
{
    private static DDOL instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 EventSystem 제거
        }
    }
}
