using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        if (CharacterManager.Instance.saveData == null)
        {
            ObjectManager.Instance.AddObject("Player", transform.position, Quaternion.identity);
        }
        else
        {
            if (CharacterManager.Instance.saveData.sceneKey == SceneLoadManager.Instance.nowSceneKey)
            {
                ObjectManager.Instance.AddObject("Player", CharacterManager.Instance.saveData.pos, Quaternion.identity);
            }
            else
            {
                ObjectManager.Instance.AddObject("Player", transform.position, Quaternion.identity);
            }
        }
    }
}
