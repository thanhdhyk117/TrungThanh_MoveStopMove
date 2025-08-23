using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        // t.OnMoveStop();
        // if (t.IsCanAttack)
        // {
        //     t.OnAttack();
        //     t.Throw();
        //     t.IsCanAttack = false;
        //     t.StartCoroutine(CoolDownAttack(Character.TIME_ON_COOLDOWN));
        // }
        // t.ChangeState(Utilities.Chance(50, 100) ? new IdleState() : new PatrolState());

        // IEnumerator CoolDownAttack(float time)
        // {
        //     yield return new WaitForSeconds(time);

        //     t.IsCanAttack = true;
        // }

        t.OnMoveStop();
        t.OnAttack();
        if (t.IsCanAttack)
        {
            t.Counter.Start(
                () =>
                {
                    t.Throw();
                    t.Counter.Start(
                    () =>
                    {
                        t.ChangeState(Utilities.Chance(50, 100) ? new IdleState() : new PatrolState());

                    }, Random.Range(0f, 1f));
                }, Character.TIME_DELAY_THROW
            );
        }
    }

    public void OnExecute(Bot t)
    {
        t.Counter.Execute();
    }

    public void OnExit(Bot t)
    {
    }


}
