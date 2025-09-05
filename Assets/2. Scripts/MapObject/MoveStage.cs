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
            CharacterManager.Instance.playerData.hp = CharacterManager.Instance.PlayerStat.CurrentHealth;
            SceneLoadManager.Instance.LoadScene(nextSceneKey);
        }
    }
}
