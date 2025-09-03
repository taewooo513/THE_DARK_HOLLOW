using UnityEngine;

public class PlayerRottenState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello From The Rotten State");
    }

    public override void UpdateState(StateManager player)
    {

    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DetractHealth();

            //player.SwitchState(player.chewedState);
        }
    }

    public override void FixedUpdateState(StateManager player)
    {

    }
}
