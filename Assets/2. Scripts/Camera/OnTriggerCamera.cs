using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerCamera : MonoBehaviour
{
   public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimation(string trigger)
    {
        if (!anim || string.IsNullOrEmpty(trigger)) return;
        anim.ResetTrigger(trigger);
        anim.SetTrigger(trigger);
    }

    void SetisMove()
    {
        /*if(CharacterManager.instance.PlayerStat.isMoved == false)
        {
            CharacterManager.instance.PlayerStat.isMoved = true;
        }
        else
        {
            CharacterManager.instance.PlayerStat.isMoved = false;
        }*/
        Debug.Log("카메라 이벤트 출력확인용");
    }
}
