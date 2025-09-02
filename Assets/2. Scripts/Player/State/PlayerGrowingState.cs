using UnityEngine;

public class PlayerGrowingState : BaseState
{
    private Vector3 startingSize = new Vector3(1, 1, 1);
    private Vector3 growScaler = new Vector3(0.1f, 0.1f, 0.1f);

    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello from the Growing State");
        player.transform.localScale = startingSize;
    }

    public override void UpdateState(StateManager player)
    {
        if (player.transform.localScale.x < 2)
        {
            player.transform.localScale += growScaler * Time.deltaTime;
        }
        else
        {
            //player.SwitchState(player.wholeState);

            // context.SwitchState(context.state);
            // context = 상태 데이터를 전달하는 역할, 상태를 전환하는 역할 
        }
    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager player)
    {

    }
}
