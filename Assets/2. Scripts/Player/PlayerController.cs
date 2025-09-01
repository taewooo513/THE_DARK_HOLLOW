using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
     * 움직이자. 
     * 
     * 움직이려면 뭐가 필요해? 
     * - 리지드 바디
     * - 이동 속도 
     * - 애니메이션
     * 
     * 플레이어가 벽이나 오브젝트에 충돌하면 막혀야돼. 
     * 충돌처리를 하려면 뭐가 필요해? 
     * - 콜라이더
     * 
     * 이동시키려면 플레이어의 입력이 필요해. 
     * 플레이어 입력을 뭘로 처리할까? 
     * 
     */

    [Header("Init Info")]
    private Rigidbody2D rigid;
    private BoxCollider2D coll;

    [Header("Player Info")]
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("누름");
    }
}
