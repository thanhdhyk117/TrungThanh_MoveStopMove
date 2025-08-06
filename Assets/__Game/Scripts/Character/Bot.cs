using System.Collections;
using UnityEngine;

public class Bot : Character
{
    public override void OnDead()
    {
        StartCoroutine(ActionBotDeath());
    }


    private IEnumerator ActionBotDeath()
    {
        Debug.Log($"{name} is dead");
        ChangeAnim(Consts.ANIM_DEAD);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        yield return new WaitForSeconds(1f);
        SimplePool.Despawn(this);
    }
}