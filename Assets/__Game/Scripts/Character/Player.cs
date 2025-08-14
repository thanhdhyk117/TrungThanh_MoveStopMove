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

    [SerializeField] private float deadzone = 0.1f;   // ngưỡng joystick
    [SerializeField] private float velocityForRun = 0.05f; // fallback bằng vận tốc thực

    protected override void HandleInput()
    {
        // Vector điều khiển từ joystick
        Vector3 direction = Vector3.forward * variableJoystick.Vertical
                            + Vector3.right * variableJoystick.Horizontal;

        // Có input nếu vượt deadzone
        bool hasInput = direction.sqrMagnitude >= (deadzone * deadzone);

        if (hasInput)
        {
            HandleMovement(direction);
        }
        else
        {
            _isMoving = false;

            // Add this field to the Player class

            float speedSq = rb != null ? rb.velocity.sqrMagnitude : 0f;

            if (speedSq >= velocityForRun * velocityForRun)
            {
                if (_currentState != EPlayerState.Moving)
                    SetState(EPlayerState.Moving);
            }
            else if (_currentState == EPlayerState.Moving)
            {
                SetState(EPlayerState.Idle);
            }
        }

        // Reset state attack nếu mất target
        if (currentTarget == null && _currentState == EPlayerState.Attack)
        {
            _timeCounter.Stop();
            SetState(EPlayerState.Idle);
        }
    }

    private void HandleMovement(Vector3 direction)
    {
        if (direction.sqrMagnitude >= (threshold * threshold)) // dùng sqrMagnitude cho ổn định
        {
            _isMoving = true;

            if (_currentState != EPlayerState.Moving)
                SetState(EPlayerState.Moving);

            Movement(direction); // hàm di chuyển bạn đã có
        }
        else
        {
            _isMoving = false;
            if (_currentState == EPlayerState.Moving)
                SetState(EPlayerState.Idle);
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