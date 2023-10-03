using UnityEngine;

public class FallingState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: FallingState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FallingState\n");
    }

    public override void OnFixedUpdate()
    {
        ApplyMovementsOnFloorFU(m_stateMachine.CurrentDirectionalInputs);
    }

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2)
    {
        //TODO MF: Explications nécessaires de ce code pour les élèves
        var vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right * inputVector2.x, Vector3.up);
        vectorOnFloor.Normalize();

        m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.InAirAccelerationValue, ForceMode.Acceleration);
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(IState currentState)
    {
        return !m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return true;
    }
}
