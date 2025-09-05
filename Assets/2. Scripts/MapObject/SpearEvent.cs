using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEvent : MonoBehaviour
{
    public GameObject coll1;
    public GameObject coll2;

    public void OnAttack()
    {
        coll1.SetActive(true);
        coll2.SetActive(false);
    }
    public void OnAttackEnd()
    {
        coll1.SetActive(false);
        coll2.SetActive(true);
    }
}
