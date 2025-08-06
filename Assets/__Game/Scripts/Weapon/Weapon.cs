using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private Character owner;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform TfMuzze;
    private float speed = 5f;

    [SerializeField] private Projectile currentProjectile;

    public void Fire()
    {
        Vector3 direction = (owner.GetTarget().TF.position - owner.TF.position).normalized;

        currentProjectile = SimplePool.Spawn<Projectile>(projectilePrefab, TfMuzze.position, Quaternion.identity);
        currentProjectile.TF.forward = direction;

        switch (currentProjectile.movementType)
        {
            case EMovementType.Towards:
                TowardsMovement towards = new TowardsMovement(direction, speed);
                currentProjectile.projectile = towards;
                break;
        }
    }

    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {
        SimplePool.Despawn(currentProjectile);
    }
}
