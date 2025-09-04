using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : BaseState
{
    private float hitCount;

    // ������ ���°� �Ƿ��� SpriteRenderer�� �ʿ��� => �̰ɷ� �� ������ ������ �� �����ϱ�
    private SpriteRenderer spriteRenderer;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Hit State");

        // ���� ���� ���� 
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
        // 0.8�ʰ� ���������� �÷��̾ ������ ����(���� ����)�� ����� 
        if(hitCount >= 0)
        {
            // 0.8�� ī���� -> 0.8�� ���� ������ ����, ���� ���� ���� 
            hitCount -= Time.deltaTime;

            // 0.1�� �������� �����̰� �ϱ� 
            if (Mathf.FloorToInt(hitCount / Constants.CountValue.INVINCIBLE_BLINK_TIME) % 2 == 0)
            {
                SetColorAlpha(spriteRenderer, Constants.ColorAlpha.ONE);
            }
            else
            {
                SetColorAlpha(spriteRenderer, Constants.ColorAlpha.HALF);
            }
        }
        // 0.8�ʰ� ������ 
        else
        {
            // ������ ���� ���� 
            SetColorAlpha(spriteRenderer, Constants.ColorAlpha.ONE);

            // ���� ���� ����
            stateMachine.gameObject.layer = Constants.LayerName.DEFAULT;

            // �ǰ� ���� -> Idle ���·� ��ȯ 
            stateMachine.PlayerController.IsHit = false;
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }

        // ��� üũ 
        if(stateMachine.PlayerController.PlayerStat.CurrentHealth <= 0.0f)
        {
            Debug.Log("�÷��̾� ����");
            SceneLoadManager.instance.LoadScene(SceneKey.titleScene);
        }
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        // 0.1�ʰ� ���� ������ 
        // �ǰݴ��� �ݴ�������� �˹� (�˹��� ĳ���� ũ���� 20%)

        // ĳ���͸� �˹��Ϸ��� ��� �ؾߵ�? 
        /*
         * �ϴ� ������ �ٵ� �ʿ���.
         * �׸��� �÷��̾ ������ �ٶ󺸴� ������ �ݴ���� ���͸� �˾ƾ���.
         * �׸��� ��. �˹��� ���� �ʿ���. 
         * -> ������ �ٵ�, �÷��̾ ������ �ٶ󺸴� ������ �ݴ�, �˹� �� 
         * 
         * 
         */

        if(stateMachine.PlayerController.collider != null)
        {
            // ���Ͱ� �÷��̾ �ٶ󺸴� ���� ����
            Vector2 direction = (stateMachine.PlayerController.transform.position - stateMachine.PlayerController.collider.transform.position).normalized;

            // �˹� �� 
            Vector2 colliderSize = stateMachine.PlayerController.collider.GetComponent<BoxCollider2D>().size;
            float x = colliderSize.x;
            float size = x * 0.2f + 2.0f;

            // ��Ʈ �ִϸ��̼�
            stateMachine.PlayerController.hitObj.SetActive(false);
            stateMachine.PlayerController.hitObj.SetActive(true);
            //GameObject go = GameObject.Instantiate(hitPrefab, stateMachine.PlayerController.transform.position);
            //// -> ���� �߻��ϴ� �� instantiate ����õ => ��� ����
            //// ���1: �������� ����� �����ϴ� ������ ��Ʈ ����� �޼���, �ҷ����� �޼��� ���� ó��
            //// ���2: �÷��̾� �ڽ����� ��Ʈ ������Ʈ�� �ּ� ������Ʈ�� ���ٰ� Ű�� ��.

            // �˹� ����
            //stateMachine.PlayerController.Rigid.AddForce(direction * CharacterManager.Instance.PlayerStat.KnockbackPower, ForceMode2D.Impulse);
            stateMachine.PlayerController.Rigid.AddForce(direction * size, ForceMode2D.Impulse);

            // ������ ó�� 
            CharacterManager.Instance.PlayerStat.TakeDamage();

            // �˹� �ߺ� ����
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
