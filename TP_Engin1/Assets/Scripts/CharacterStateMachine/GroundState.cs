using UnityEngine;

public class GroundState : CharacterState
{
    private const float STUN_DURATION = 1.5f;
    private float m_currentStateDuration;

    public override void OnEnter()
    {
        m_stateMachine.OnStunStimuliReceived = false;
        m_stateMachine.Animator.SetBool("IsStun", true);
        m_currentStateDuration = STUN_DURATION;

        Debug.Log("Enter state: GroundState\n");
    }

    public override void OnExit()
    {
        m_stateMachine.Animator.SetBool("IsStun", false);
        Debug.Log("Exit state: GroundState\n");
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.FixedUpdateQuickDeceleration();
    }

    public override void OnUpdate()
    {
        m_currentStateDuration -= Time.deltaTime;
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.OnStunStimuliReceived;

    }

    public override bool CanExit()
    {
        return m_currentStateDuration < 0;
    }

    private void FixedUpdateQuickDeceleration()
    {
        var oppositeDirectionForceToApply = -m_stateMachine.RB.velocity *
        m_stateMachine.DecelerationValue * Time.fixedDeltaTime;
        m_stateMachine.RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }

}
