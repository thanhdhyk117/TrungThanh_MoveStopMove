using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Transform skin;
    
    [Header("Time - Control Attack")]
    private TimeCounter _timeCounter;

    [SerializeField] private float timeToDelay = 1.0f;
    [SerializeField] private float threshold = 0.3f;
    
    private EPlayerState _currentState = EPlayerState.Idle;
    
    [Header("Test target")]
    [SerializeField] private bool isTestTarget = false;

    private void Start()
    {
        _timeCounter = new TimeCounter();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            
            Movement(direction);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChangeAnim(Consts.ANIM_IDLE);
        }
        
        // PlayerAction();
    }

    private void Movement(Vector3 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
        
        if (direction.magnitude > 0.1f)
        {
            skin.forward = direction;
            ChangeAnim(Consts.ANIM_RUN);
        }
    }

    private void PlayerAction(float joystickValue)
    {
        bool isMoving = joystickValue > threshold;
        if (isMoving)
        {
            _timeCounter.Stop();
            SetState(EPlayerState.Moving);
        }
        else
        {
            if(!_timeCounter.IsRunning)
                SetState(EPlayerState.Idle);
        }

        if (!isMoving && !_timeCounter.IsRunning)
        {
            _timeCounter.Run(Attack, timeToDelay);
        }
        
        _timeCounter.Excute(Time.deltaTime);
    }

    private void Attack()
    {
        SetState(EPlayerState.Attack);
        Debug.Log("Attack action performed");
    }

    private void SetState(EPlayerState state)
    {
        if(_currentState == state) return;
        
        _currentState = state;
        switch (state)
        {
            case EPlayerState.Attack:
                Debug.Log("Attack is calling");
                break;
            case EPlayerState.Idle:
                Debug.Log("Idle is calling");
                break;
            case EPlayerState.Moving:
                Debug.Log("Moving is calling");
                break;
            default:
                break;
        }
    }
}
