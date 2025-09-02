using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossStat bossStat;
    public BossStateMachine bossState;
    private void Awake()
    {
       CharacterManager.instance.Boss = this;
        bossState = GetComponent<BossStateMachine>();
        bossStat = GetComponent<BossStat>();
    }
}
