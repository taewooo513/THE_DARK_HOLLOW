using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossStat bossStat;

    private void Awake()
    {
        bossStat = GetComponent<BossStat>();
    }
}
