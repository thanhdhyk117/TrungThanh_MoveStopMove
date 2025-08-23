using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoomerang : Bullet
{
    private Vector3 target;
    public enum State { Forward, Backward, Stop }

    private State state;

    [SerializeField] Transform child;

    public override void OnInit(Character character, Vector3 target, float size)
    {
        base.OnInit(character, target, size);
        this.target = (target - character.TF.position).normalized * (Character.ATTACK_RANGE + 1) * size + character.TF.position;
        state = State.Forward;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Forward:
                TF.position = Vector3.MoveTowards(TF.position, this.target, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(TF.position, target) < 0.1f)
                {
                    state = State.Backward;
                }
                child.Rotate(Vector3.forward * -6, Space.Self);
                break;

            case State.Backward:
                TF.position = Vector3.MoveTowards(TF.position, this.character.TF.position, moveSpeed * Time.deltaTime);
                if (character.IsDead || Vector3.Distance(TF.position, this.character.TF.position) < 0.1f)
                {
                    OnDespawn();
                }
                child.Rotate(Vector3.forward * -6, Space.Self);

                break;
        }
    }

    protected override void OnStop()
    {
        base.OnStop();
        state = State.Stop;
        Invoke(nameof(OnDespawn), 2f);
    }
}
