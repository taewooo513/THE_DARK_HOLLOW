using UnityEngine;

public class PlayerIdleState : BaseState
{
    //private PlayerController playerController; 

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Idle State");
        //this.playerController = stateMachine.PlayerController;

        ChangeSpeed();
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {

    }

    private void ChangeSpeed()
    {
        CharacterManager.instance.PlayerStat.SpeedModifier = 1.0f;
        //this.playerController.SpeedModifier = 1.0f;
    }
}
