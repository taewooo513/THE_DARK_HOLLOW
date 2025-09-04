using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    void Awake()
    {
        CameraManager.Instance.collider2D = this.GetComponent<CompositeCollider2D>();
    }
}
