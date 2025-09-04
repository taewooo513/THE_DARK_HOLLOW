using UnityEngine;

public class PlayerIdleState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Idle State");
        this.playerController = stateMachine.PlayerController;

        // ���� ���� ���� 
        stateMachine.SetPreState(stateMachine);

        // Idle Animation
        playerController.AnimationController.Move(Vector2.zero);
        playerController.AnimationController.Run(Vector2.zero);

        ChangeSpeed();
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // �������� �¾�����
        if (playerController.IsHit)
        {
            // Hit���·� ���� 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Hit));
        }

        // �߰�
        // ����Ű�� ��� ������ ������
        if (playerController.IsMoving)
        {
            // ���� ���°� Jump�̰�, ���� ���� ���°� Run�̸� 
            if(stateMachine.GetPreState().ToString().Equals(Constants.State.JUMP) &&
                stateMachine.GetStateBeforeJump().ToString().Equals(Constants.State.RUN))
            {
                // �޸��� ���·� ��ȯ
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
            }

            // ���� ���°� Jump�̰�, ���� ���� ���°� Move�̸� 
            else if (stateMachine.GetPreState().ToString().Equals(Constants.State.JUMP) &&
                stateMachine.GetStateBeforeJump().ToString().Equals(Constants.State.MOVE))
            {
                // �ȱ� ���·� ��ȯ
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
            }
        }
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
    }
}
