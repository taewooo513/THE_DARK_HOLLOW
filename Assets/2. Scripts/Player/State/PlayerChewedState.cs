using UnityEngine;

public class PlayerChewedState : BaseState
{
    private float destroyCountdown = 5.0f;

    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello From The Chewed State");

        // 먹는 상태에 진입했을 때, 애니메이션도 적용하게 만들 수 있다. 
        //Animator animator = player.GetComponent<Animator>();
        //animator.Play("Base Layer.eat", 0, 0);
    }

    public override void UpdateState(StateManager player)
    {
        if (destroyCountdown > 0)
        {
            destroyCountdown -= Time.deltaTime;
        }
        else
        {
            Object.Destroy(player.gameObject);
        }
    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager player)
    {

    }
}
