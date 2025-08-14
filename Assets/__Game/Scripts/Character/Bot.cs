using System;
public class Bot : Character
{
    private PointSpawn _point;
    private Level _level;

    // Sự kiện để PointSpawn lắng nghe
    public event Action<Bot> OnDespawned;
    public event Action<Bot> OnDisabled;

    public void BindSpawnPoint(PointSpawn p, Level level)
    {
        _point = p;
        _level = level;
    }
    protected override void HandleDeath()
    {
        base.HandleDeath();
        StartCoroutine(DeathSequence(1f));
    }

    protected override void HandleCombat()
    {
        base.HandleCombat();
    }

    /// <summary>
    /// Được Level gọi khi muốn despawn bot (thay vì gọi SimplePool.Despawn trực tiếp).
    /// </summary>
    public void Despawn()
    {
        // Báo cho point trước khi biến mất
        _point?.ForceRelease();

        // Phát sự kiện cho bất kỳ ai đang lắng nghe (PointSpawn)
        OnDespawned?.Invoke(this);

        // Thực sự despawn qua pool
        SimplePool.Despawn(this);
    }

    private void OnDisable()
    {
        // Khi bị disable (ví dụ pool SetActive(false)), vẫn phát tín hiệu
        OnDisabled?.Invoke(this);
    }

    /// <summary>
    /// API cho Level nếu vẫn muốn gọi “trả chỗ” từ phía bot.
    /// </summary>
    public void ReleaseSpawnPoint()
    {
        _point?.ForceRelease();
        _point = null;
    }


    public override void OnDead()
    {
        base.OnDead();

        _level?.DespawnBot(this);
    }
}