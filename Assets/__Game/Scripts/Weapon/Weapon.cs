using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private Character owner;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform TfMuzze;
    private float speed = 5f;
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
