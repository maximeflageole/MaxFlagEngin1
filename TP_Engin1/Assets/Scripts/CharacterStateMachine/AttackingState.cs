using UnityEngine;

public class AttackingState : CharacterState
{
    private const float ATTACK_DURATION = 0.6f;
    private float m_currentStateDuration;
    
    public override void OnEnter()
    {
        m_stateMachine.Animator.SetTrigger("Attacks");
        m_currentStateDuration = ATTACK_DURATION;
        Debug.Log("Enter state: AttackingState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: AttackingState\n");
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
        if (currentState is FreeState)
        {
            return Input.GetKeyDown(KeyCode.Mouse0);
        }
        return false;
    }

    public override bool CanExit()
    {
        return m_currentStateDuration < 0;
    }
}
