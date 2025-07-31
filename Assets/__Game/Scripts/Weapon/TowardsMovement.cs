using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsMovement : IProjectile
{
    private Vector3 direction;
    private float speed;
    
    public TowardsMovement(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }
    
    public void Move(Projectile projectile)
    {
     projectile.transform.Translate(direction * speed * Time.deltaTime);   
    }
}
