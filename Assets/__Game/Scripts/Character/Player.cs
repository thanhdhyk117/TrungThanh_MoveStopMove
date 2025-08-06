using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Transform skin;

    [SerializeField] private GameObject bulletPrefab; // Prefab for the bullet to be instantiated on attack
    public Vector3 Direction => transform.eulerAngles;

    [Header("Time - Control Attack")]
    private TimeCounter _timeTimeCounter;

    [SerializeField] private float timeToDelay = 1f; // Time to initialize anim "Attack"  before attack
    [SerializeField] private float attackCooldown = 1.5f; // Cooldown period after attack
    [SerializeField] private float threshold = 0.1f;

    private EPlayerState _currentState = EPlayerState.Idle;
    private bool _canAttack = true; // Tracks if player can attack (not in cooldown)

    private void Start()
    {
        _timeTimeCounter = new TimeCounter();
    }

    private void Update()
    {
        var direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        if (Input.GetMouseButton(0))
        {
            Movement(direction);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChangeAnim(Consts.ANIM_IDLE);
        }

        if (currentTarget != null)
        {
            PlayerAction(direction.magnitude);
        }
    }

    private void Movement(Vector3 direction)
    {
        if (direction.magnitude > threshold)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            ChangeAnim(Consts.ANIM_RUN);
            skin.forward = direction;
        }
        else
        {
            ChangeAnim(Consts.ANIM_IDLE);
        }
    }

    public override void OnDead()
    {

    }

    private void PlayerAction(float joystickValue)
    {
        bool isMoving = joystickValue > threshold;
        if (isMoving)
        {
            _timeTimeCounter.Stop();
            SetState(EPlayerState.Moving);
        }
        else
        {
            if (!_timeTimeCounter.IsRunning && !isAttacking)
            {
                SetState(EPlayerState.Idle);
            }
        }

        // Prepare attack when target is set and not moving
        if (currentTarget != null && !isMoving && !_timeTimeCounter.IsRunning && CanAttack())
        {
            TF.LookAt(currentTarget.TF.position);
            SetState(EPlayerState.Attack);
            _timeTimeCounter.Run(Attack, timeToDelay);
        }

        _timeTimeCounter.Excute(Time.deltaTime);
    }

    private bool CanAttack()
    {
        return _canAttack && _currentState != EPlayerState.Attack && _currentState != EPlayerState.Moving;
    }

    private void Attack()
    {
        _canAttack = false; // Prevent re-attack
        isAttacking = false;

        currentWeapon.Fire(); // Fire the weapon

        StartCoroutine(AttackCooldown()); // Start cooldown period
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true; // Allow attacking again after cooldown
        Debug.Log("Attack cooldown finished, can attack again");
    }

    private void SetState(EPlayerState state)
    {
        if (_currentState == state) return;

        _currentState = state;
        switch (state)
        {
            case EPlayerState.Attack:

                ChangeAnim(Consts.ANIM_ATTACK);
                isAttacking = true;
                break;
            case EPlayerState.Idle:
                ChangeAnim(Consts.ANIM_IDLE);
                isAttacking = false;
                break;
            case EPlayerState.Moving:
                isAttacking = false;
                break;
            default:
                break;
        }
    }
}