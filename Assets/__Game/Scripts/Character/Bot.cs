using UnityEngine;

public class Bot : Character
{
    [SerializeField] private PointSpawn pointSpawn;

    protected override void HandleDeath()
    {
        base.HandleDeath();
        // LevelManager.Instance.FreeUpSpawnPoint(pointSpawn);
        StartCoroutine(DeathSequence(1f));
    }

    protected override void HandleCombat()
    {
        base.HandleCombat();
    }
}