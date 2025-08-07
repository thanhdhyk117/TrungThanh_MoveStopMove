using System.Collections;
using UnityEngine;

public class Bot : Character
{
    protected override void HandleDeath()
    {
        base.HandleDeath();
        StartCoroutine(DeathSequence(1f));
    }
    
    protected override void HandleCombat()
    {
        base.HandleCombat();
    }
}