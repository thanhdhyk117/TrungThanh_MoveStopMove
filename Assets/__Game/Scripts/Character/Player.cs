using UnityEngine;

public class Player : Character
{
    [Header("Player Input")]
    [SerializeField] private VariableJoystick variableJoystick;

    private EPlayerState _currentState = EPlayerState.Idle;
    private bool _isMoving = false;

    private void Start()
    {
        OnInit();
    }

    protected override void HandleInput()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        bool hasInput = Input.GetMouseButton(0);

        if (hasInput)
        {
            HandleMovement(direction);
        }
        else
        {
            _isMoving = false;
            if (_currentState == EPlayerState.Moving)
            {
                SetState(EPlayerState.Idle);
            }
        }

        // Update state based on target availability
        if (currentTarget == null && _currentState == EPlayerState.Attack)
        {
            _timeCounter.Stop();
            SetState(EPlayerState.Idle);
        }
    }

    private void HandleMovement(Vector3 direction)
    {
        if (direction.magnitude >= threshold)
        {
            _isMoving = true;
            Movement(direction);

            if (_currentState != EPlayerState.Moving)
            {
                SetState(EPlayerState.Moving);
            }
        }
        else
        {
            _isMoving = false;
            if (_currentState == EPlayerState.Moving)
            {
                SetState(EPlayerState.Idle);
            }
        }
    }

    protected override bool CanAttack()
    {
        return base.CanAttack() &&
               !_isMoving &&
               _currentState != EPlayerState.Moving;
    }

    protected override void PrepareAttack()
    {
        if (!CanAttack()) return;

        SetState(EPlayerState.Attack);
        base.PrepareAttack();
    }

    protected override void OnAttackComplete()
    {
        base.OnAttackComplete();
        if (_currentState == EPlayerState.Attack)
        {
            SetState(EPlayerState.Idle);
        }
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
        _timeCounter?.Stop();
        SetState(EPlayerState.Idle);
    }

    private void SetState(EPlayerState state)
    {
        if (_currentState == state) return;

        _currentState = state;

        switch (state)
        {
            case EPlayerState.Attack:
                break;

            case EPlayerState.Idle:
                ChangeAnim(Consts.ANIM_IDLE);
                break;

            case EPlayerState.Moving:
                ChangeAnim(Consts.ANIM_RUN);
                break;
        }
    }
}