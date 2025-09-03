using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStage : MonoBehaviour
{
    [SerializeField]
    private string nextSceneKey;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out PlayerController player))
        {
            SceneLoadManager.Instance.LoadScene(nextSceneKey);
        }
    }
}
