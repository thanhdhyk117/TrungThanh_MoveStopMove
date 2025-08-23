using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    protected Character character;
    protected float moveSpeed = 6.0f;
    protected bool isRunning;

    public virtual void OnInit(Character character, Vector3 target, float size)
    {
        this.character = character;
        TF.forward = (target - TF.position).normalized;
        isRunning = true;
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    protected virtual void OnStop() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_CHARACTER))
        {
            Character hit = other.GetComponent<Character>();

            if (hit != null && hit != character)
            {
                hit.OnHit(
                    () =>
                    {
                        character.AddScore(1);
                        SimplePool.Despawn(this);
                    });
            }
        }

        if (other.CompareTag(Constant.TAG_OBSTACLE))
        {
            OnStop();
        }

    }

}
