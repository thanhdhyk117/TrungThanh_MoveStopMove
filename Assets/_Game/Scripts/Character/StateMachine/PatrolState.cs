using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.SetDestination(LevelManager.Ins.RandomPoint());

    }

    public void OnExecute(Bot t)
    {
        if (t.IsDestination)
        {
            t.ChangeState(new IdleState());
        }

        Character target = t.GetTargetInRange();

        if (target != null && !target.IsDead)
        {
            if (Utilities.Chance(5, 100)) t.ChangeState(new AttackState());
        }
    }

    public void OnExit(Bot t)
    {

    }

}
