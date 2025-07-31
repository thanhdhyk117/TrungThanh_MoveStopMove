using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private Character owner;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform TfMuzze;
    private float speed = 5f;


    public void Fire()
    {
        Projectile projectile = SimplePool.Spawn<Projectile>(projectilePrefab, TfMuzze.position, TfMuzze.rotation);
        Vector3 direction = (owner.GetTarget().TF.position - owner.TF.position).normalized;

        switch (projectile.movementType)
        {
            case EMovementType.Towards:
                TowardsMovement towards = new TowardsMovement(direction, speed);
                projectile.iProjectile = towards;
                break;
        }
    }
    
    
    
    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {

    }

    private void Update()
    {

    }
}
