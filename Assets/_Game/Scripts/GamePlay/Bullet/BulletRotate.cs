using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotate : Bullet
{
    public const float TIME_ALIVE = 2f;
    [SerializeField] Transform child;
    CounterTime counterTime = new CounterTime();

    public override void OnInit(Character character, Vector3 target, float size)
    {
        base.OnInit(character, target, size);
        counterTime.Start(OnDespawn, TIME_ALIVE);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            TF.Translate(TF.forward * moveSpeed * Time.deltaTime, Space.World);
            child.Rotate(Vector3.up * -6, Space.Self);
        }
    }

    protected override void OnStop()
    {
        base.OnStop();
        isRunning = false;
    }
}
