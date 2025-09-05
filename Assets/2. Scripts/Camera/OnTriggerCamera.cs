using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerCamera : MonoBehaviour
{
    [SerializeField] GameObject camera;

    void SetisMove()
    {
        if(CharacterManager.instance.PlayerStat.isMoved == true)
        {
            CharacterManager.instance.PlayerStat.isMoved = false;
        }
        else
        {
            CharacterManager.instance.PlayerStat.isMoved = true;
            camera.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void SetBossEFX()
    {
        SoundManager.Instance.PlayEFXSound("BossAppear_EFX");
    }
}
