using UnityEngine;

public class Projectile : GameUnit
{
    public IProjectile projectile;
    public EMovementType movementType;

    public Character triggerCharacter;

    private void Update()
    {
        projectile.Move(this);
    }

    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        triggerCharacter = Cache.GetCharacter(other);

        if (triggerCharacter != null)
        {
            OnDespawn();
            triggerCharacter.OnDead();
            triggerCharacter = null;
        }
    }
}


public enum EMovementType
{
    None = 0,
    Towards = 1
}