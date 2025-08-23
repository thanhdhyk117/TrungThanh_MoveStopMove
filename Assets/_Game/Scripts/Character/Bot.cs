using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] protected NavMeshAgent agent;

    protected IState<Bot> currentState;
    private CounterTime counter = new CounterTime();
    public CounterTime Counter => counter;
    private Vector3 destination;


    public float walkRadius = 10.0f;

    private bool IsCanRunning => (GameManager.Ins.IsState(GameState.GamePlay) || GameManager.Ins.IsState(GameState.Revive) || GameManager.Ins.IsState(GameState.Setting));
    public bool IsDestination => Vector3.Distance(TF.position, destination) - Mathf.Abs(TF.position.y - destination.y) < 0.1f;

    public override void OnInit()
    {
        base.OnInit();
        ResetAnim();
    }

    private void Update()
    {
        if (IsCanRunning && currentState != null && !IsDead)
        {
            currentState.OnExecute(this);
        }
    }

    public void SetDestination(Vector3 point)
    {
        destination = point;
        agent.enabled = true;
        agent.SetDestination(destination);
        ChangeAnim(Constant.ANIM_RUN);
    }

    public void ChangeState(IState<Bot> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }

    }

    public override void OnDeath()
    {
        OnMoveStop();
        base.OnDeath();
        ChangeState(null);
        Invoke(nameof(OnDespawn), TIME_ON_DEATH);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        SimplePool.Despawn(this);
        CancelInvoke();
    }
    public override void OnMoveStop()
    {
        base.OnMoveStop();
        agent.enabled = false;
        ChangeAnim(Constant.ANIM_IDLE);
    }

    public override void Throw()
    {
        base.Throw();
    }

    public override void WearClothes()
    {
        base.WearClothes();

        //change random 
        ChangeSkin(SkinType.SKIN_Normal);
        ChangeWeapon(Utilities.RandomEnumValue<WeaponType>());
        ChangeHat(Utilities.RandomEnumValue<HatType>());
        ChangeAccessory(Utilities.RandomEnumValue<AccessoryType>());
        ChangePant(Utilities.RandomEnumValue<PantType>());
    }
}
