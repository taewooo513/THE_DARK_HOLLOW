using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIcon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string damagedBoolName = "IsDamaged";
    [SerializeField] private float destroyDelay = 0.8f;

    // 잃을 때 호출됨
    public void IconDestroy()
    {
        if (animator != null)
        {
            animator.SetBool(damagedBoolName, true);
        }
        // 애니메이션이 교체된 상태에서 0.8초 후 파괴
        Destroy(gameObject, destroyDelay);
    }
}
