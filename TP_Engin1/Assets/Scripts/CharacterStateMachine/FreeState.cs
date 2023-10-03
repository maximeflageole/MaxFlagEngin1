using UnityEngine;

public class FreeState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        FixedUpdateRotateWithCamera();

         var inputVector2 = Vector2.zero;
        GetInputs(ref inputVector2);
        m_stateMachine.CurrentDirectionalInputs = inputVector2;

        if (inputVector2 ==  Vector2.zero)
        {
            FixedUpdateQuickDeceleration();
            return;
        }

        ApplyMovementsOnFloorFU(inputVector2);
    }
    
    private void GetInputs(ref Vector2 inputVector2)
    {
        //Pourquoi est-ce que cette méthode s'appelle Get même si elle n'a pas de valeur de retour?
        // Ce n'est pas une erreur!
        if (Input.GetKey(KeyCode.W))
        {
            inputVector2 += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector2 += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector2 += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector2 += Vector2.right;
        }
    }

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2)
    {
        //TODO MF: Explications nécessaires de ce code pour les élèves
        var vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right * inputVector2.x, Vector3.up);
        vectorOnFloor.Normalize();

        m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.AccelerationValue, ForceMode.Acceleration);

        var currentMaxSpeed = m_stateMachine.GetCurrentMaxSpeed();

        if (m_stateMachine.RB.velocity.magnitude > currentMaxSpeed)
        {
           m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
           m_stateMachine.RB.velocity *= currentMaxSpeed;
        }
    }

    private void FixedUpdateQuickDeceleration()
    {
        var oppositeDirectionForceToApply = -m_stateMachine.RB.velocity * 
        m_stateMachine.DecelerationValue * Time.fixedDeltaTime;
        m_stateMachine.RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }

    private void FixedUpdateRotateWithCamera()
    {
        var forwardCamOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        m_stateMachine.RB.transform.LookAt(forwardCamOnFloor + m_stateMachine.RB.transform.position);
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FreeState\n");
    }

    public override bool CanEnter()
    {
        //Je ne peux entrer dans le FreeState que si je touche le sol
        return m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return true;
    }
}
