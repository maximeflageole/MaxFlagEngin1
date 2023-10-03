using UnityEngine;

public class HitState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: HitState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: HitState\n");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(IState currentState)
    {
        return false;
    }

    public override bool CanExit()
    {
        return true;
    }
}
