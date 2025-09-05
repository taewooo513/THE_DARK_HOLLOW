using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SavePoint : MonoBehaviour
{
    public GameObject activePoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerStat stat))
        {
            activePoint.SetActive(true);
            SaveData saveData = new SaveData();
            saveData.pos = transform.position;
            saveData.hp = stat.CurrentHealth;
            saveData.sceneKey = SceneLoadManager.Instance.nowSceneKey;
            CharacterManager.Instance.saveData = saveData;
        }
    }
}
