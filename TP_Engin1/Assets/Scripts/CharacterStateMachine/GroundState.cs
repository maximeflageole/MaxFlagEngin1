using UnityEngine;

public class GroundState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: GroundState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: GroundState\n");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter()
    {
        return false;
    }

    public override bool CanExit()
    {
        return true;
    }
}
