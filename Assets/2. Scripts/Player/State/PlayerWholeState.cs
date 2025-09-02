using UnityEngine;

public class PlayerWholeState : BaseState
{
    private float rottenCountDown = 10.0f;

    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello From The Whole State");
        player.GetComponent<Rigidbody2D>().gravityScale = 5.0f; // test 
    }

    public override void UpdateState(StateManager player)
    {
        if(rottenCountDown >= 0)
        {
            rottenCountDown -= Time.deltaTime;
        }
        else
        {
            //player.SwitchState(player.rottenState);
        }
    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddHealth();

            //player.SwitchState(player.chewedState);
        }
    }

    public override void FixedUpdateState(StateManager player)
    {

    }
}
