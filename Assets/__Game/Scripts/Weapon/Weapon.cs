using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private Character owner;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform TfMuzze;
    private float speed = 5f;

    [SerializeField] private GameObject skinWeapon;

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

        Debug.Log($"Fire projectile: {currentProjectile.name} with direction: {direction}");

        ShowHideSkinWeapon(false);
    }

    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {
        SimplePool.Despawn(currentProjectile);
    }

    public void ShowHideSkinWeapon(bool isEnable)
    {
        if (skinWeapon != null)
        {
            skinWeapon.SetActive(isEnable);
        }
    }
}
