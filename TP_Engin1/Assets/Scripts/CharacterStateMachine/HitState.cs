using UnityEngine;

public class HitState : CharacterState
{
    private const float HIT_DURATION = 0.4f;
    private float m_currentStateDuration;

    public override void OnEnter()
    {
        m_currentStateDuration = HIT_DURATION;
        m_stateMachine.OnHitStimuliReceived = false;
        m_stateMachine.Animator.SetTrigger("OnHit");
        Debug.Log("Enter state: HitState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: HitState\n");
    }

    public override void OnFixedUpdate()
    {
        FixedUpdateQuickDeceleration();
    }

    public override void OnUpdate()
    {
        m_currentStateDuration -= Time.deltaTime;
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.OnHitStimuliReceived;
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
