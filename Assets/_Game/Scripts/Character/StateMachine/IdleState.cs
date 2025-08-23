using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.OnMoveStop();
        t.Counter.Start(() => t.ChangeState(new PatrolState()), Random.Range(0f, 2f));
    }

    public void OnExecute(Bot t)
    {
        if (Utilities.Chance(50, 100))
        {
            t.ChangeState(new PatrolState());
        }

    }

    public void OnExit(Bot t)
    {

    }

}
