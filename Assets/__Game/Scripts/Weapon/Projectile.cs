using System;
using UnityEngine;

public class Projectile : GameUnit
{
    public IProjectile iProjectile; 
    public EMovementType movementType;

    private void Update()
    {
        iProjectile.Move(this);
    }

    public override void OnInit()
    {
        throw new NotImplementedException();
    }

    public override void OnDespawn()
    {
        throw new NotImplementedException();
    }
}


public enum EMovementType
{
    None = 0,
    Towards = 1
}