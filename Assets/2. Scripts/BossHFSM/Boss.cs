using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossStat bossStat;
    public BossController controller;

    private void Awake()
    {
        CharacterManager.Instance.Boss = this;
        bossStat = GetComponent<BossStat>();
        controller  = GetComponent<BossController>();
    }

}
