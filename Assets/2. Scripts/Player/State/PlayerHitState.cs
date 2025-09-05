using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : BaseState
{
    private float hitCount;

    // 반투명 상태가 되려면 SpriteRenderer가 필요함 => 이걸로 색 투명도를 조절할 수 있으니까
    private SpriteRenderer spriteRenderer;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Hit State");

        stateMachine.PlayerController.Rigid.gravityScale = 1.0f;

        // 피격 사운드 
        SoundManager.instance.PlayEFXSound(Constants.SFX.PLAYER_HIT);

        // 이전 상태 저장 
        stateMachine.SetPreState(stateMachine);

        Debug.Log("Set hit count");
        hitCount = Constants.CountValue.HIT;

        Debug.Log("Set SpriteRenderer alpha(0.5)");
        spriteRenderer = stateMachine.PlayerController.SpriteRenderer;
        SetColorAlpha(spriteRenderer, Constants.ColorAlpha.HALF);

        Debug.Log("Switch default layer to invincible layer");
        stateMachine.gameObject.layer = Constants.LayerName.INVINCIBLE;
    }

    private void SetColorAlpha(SpriteRenderer spriteRenderer, float value)
    {
        Color color = spriteRenderer.color;
        color.a = value;
        spriteRenderer.color = color;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 0.8초가 지날때까지 플레이어를 반투명 상태(무적 상태)로 만들기 
        if(hitCount >= 0)
        {
            // 0.8초 카운팅 -> 0.8초 동안 반투명 상태, 무적 상태 유지 
            hitCount -= Time.deltaTime;

            // 0.1초 간격으로 깜빡이게 하기 
            if (Mathf.FloorToInt(hitCount / Constants.CountValue.INVINCIBLE_BLINK_TIME) % 2 == 0)
            {
                SetColorAlpha(spriteRenderer, Constants.ColorAlpha.ONE);
            }
            else
            {
                SetColorAlpha(spriteRenderer, Constants.ColorAlpha.HALF);
            }
        }
        // 0.8초가 지나면 
        else
        {
            // 반투명 상태 해제 
            SetColorAlpha(spriteRenderer, Constants.ColorAlpha.ONE);

            // 무적 상태 해제
            stateMachine.gameObject.layer = Constants.LayerName.DEFAULT;

            // 피격 상태 -> Idle 상태로 전환 
            stateMachine.PlayerController.IsHit = false;
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }

        // 사망 체크 
        if(stateMachine.PlayerController.PlayerStat.CurrentHealth <= 0.0f)
        {
            Debug.Log("플레이어 죽음");
            SoundManager.Instance.PlayEFXSound(Constants.SFX.PLAYER_DEAD);
            SceneLoadManager.instance.LoadScene(SceneKey.startScene);
        }
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        if(stateMachine.PlayerController.collider != null)
        {
            // 몬스터가 플레이어를 바라보는 방향 벡터
            Vector2 direction = (stateMachine.PlayerController.transform.position - stateMachine.PlayerController.collider.transform.position).normalized;

            // 넉백 힘 
            Vector2 colliderSize = stateMachine.PlayerController.collider.GetComponent<BoxCollider2D>().size;
            float x = colliderSize.x;
            float size = x * 0.2f + 2.0f;

            // 히트 애니메이션
            stateMachine.PlayerController.hitObj.SetActive(false);
            stateMachine.PlayerController.hitObj.SetActive(true);
            //GameObject go = GameObject.Instantiate(hitPrefab, stateMachine.PlayerController.transform.position);
            //// -> 자주 발생하는 건 instantiate 비추천 => 비용 때문
            //// 방법1: 프리팹을 만들고 관리하는 곳에서 히트 만드는 메서드, 불러오는 메서드 만들어서 처리
            //// 방법2: 플레이어 자식으로 히트 오브젝트를 둬서 오브젝트를 껐다가 키면 됨.

            // 넉백 적용
            stateMachine.PlayerController.Rigid.AddForce(direction * size, ForceMode2D.Impulse);

            // 데미지 처리 
            CharacterManager.Instance.PlayerStat.TakeDamage();

            // 넉백 중복 방지
            stateMachine.PlayerController.collider = null;
        }
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }
}
