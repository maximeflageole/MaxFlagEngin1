using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerStateMachine : BaseStateMachine<CharacterState>, IDamageable
{
    public Camera Camera { get; private set; }
    [field:SerializeField]
    public Rigidbody RB { get; private set; }
    [field:SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public float InAirAccelerationValue { get; private set; } = 0.2f;
    [field: SerializeField]
    public float AccelerationValue { get; private set; }
    [field: SerializeField]
    public float DecelerationValue { get; private set; } = 0.3f;
    [field: SerializeField]
    public float MaxForwardVelocity { get; private set; }
    [field: SerializeField]
    public float MaxSidewaysVelocity { get; private set; }
    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; }
    private Vector2 CurrentRelativeVelocity { get; set; }
    public Vector2 CurrentDirectionalInputs { get; private set; }
    public bool OnHitStimuliReceived { get; set; } = false;
    public bool OnStunStimuliReceived { get; set; } = false;

    [field: SerializeField]
    public float JumpIntensity { get; private set; } = 1000.0f;

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;

    protected override void CreatePossibleStates() 
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new FallingState());
        m_possibleStates.Add(new HitState());
        m_possibleStates.Add(new GroundState());
        m_possibleStates.Add(new AttackingState());
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

        Camera = Camera.main;
    }

    protected override void Update()
    {
        base.Update();

        UpdateAnimatorValues();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        SetDirectionalInputs();
        base.FixedUpdate();
        Set2dRelativeVelocity();
    }

    public bool IsInContactWithFloor()
    {
        return m_floorTrigger.IsOnFloor;
    }

    private void UpdateAnimatorValues()
    {
        //Aller chercher ma vitesse actuelle
        //Communiquer directement avec mon Animator

        Animator.SetFloat("MoveX", CurrentRelativeVelocity.x / GetCurrentMaxSpeed());
        Animator.SetFloat("MoveY", CurrentRelativeVelocity.y / GetCurrentMaxSpeed());
        Animator.SetBool("TouchGround", m_floorTrigger.IsOnFloor);
    }

    private void Set2dRelativeVelocity()
    {
        Vector3 relativeVelocity = RB.transform.InverseTransformDirection(RB.velocity);

        CurrentRelativeVelocity = new Vector2(relativeVelocity.x, relativeVelocity.z);
    }

    public float GetCurrentMaxSpeed()
    {
        //On va aller chercher la composante réelle de nos vitesses de manière très simple.
        //On va prendre notre vitesse en z (l'avant/arrière) et en x (côtés) et notre
        // vitesse actuelle réelle dépendra du résultat

        if (Mathf.Approximately(CurrentDirectionalInputs.magnitude, 0))
        {
            return MaxForwardVelocity;
        }

        var normalizedInputs = CurrentDirectionalInputs.normalized;

        var currentMaxVelocity = Mathf.Pow(normalizedInputs.x, 2) * MaxSidewaysVelocity;

        if (normalizedInputs.y > 0)
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxForwardVelocity;
        }
        else
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxBackwardVelocity;
        }

        return currentMaxVelocity;
    }

    public void SetDirectionalInputs()
    {
        CurrentDirectionalInputs = Vector2.zero;

        //Pourquoi est-ce que cette méthode s'appelle Get même si elle n'a pas de valeur de retour?
        // Ce n'est pas une erreur!
        if (Input.GetKey(KeyCode.W))
        {
            CurrentDirectionalInputs += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            CurrentDirectionalInputs += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            CurrentDirectionalInputs += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            CurrentDirectionalInputs += Vector2.right;
        }
    }

    public void ReceiveDamage(EDamageType damageType)
    {
        if (damageType == EDamageType.Normal)
        {
            OnHitStimuliReceived = true;
        }
        if (damageType == EDamageType.Stunning)
        {
            OnStunStimuliReceived = true;
        }
    }

    public void FixedUpdateQuickDeceleration()
    {
        var oppositeDirectionForceToApply = -RB.velocity *
        DecelerationValue * Time.fixedDeltaTime;
        RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }
}
