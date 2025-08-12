using UnityEngine;

public class PointSpawn : MonoBehaviour
{
    [SerializeField] private Level level;
    private int occupants = 0;
    private Bot currentBot;

    public bool IsFree => occupants == 0;

    // Gọi từ Level khi init (đảm bảo level != null)
    public void Init(Level lvl)
    {
        level = lvl;
        if (IsFree) level.AddPoint(this);   // khởi tạo là rảnh
    }

    private void OnTriggerEnter(Collider other)
    {
        var bot = Cache.GetCharacter(other) as Bot;
        if (bot == null) return;

        if (occupants == 0)
        {
            // lần đầu có occupant → point BUSY
            level.RemovePoint(this);
        }

        occupants++;

        // chỉ gắn currentBot nếu chưa có (1 bot/point)
        if (currentBot == null)
        {
            AttachBot(bot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var bot = Cache.GetCharacter(other) as Bot;
        if (bot == null) return;

        occupants = Mathf.Max(0, occupants - 1);

        if (currentBot == bot)
        {
            DetachCurrentBot();
        }

        if (occupants == 0)
        {
            // không còn ai đứng → point FREE
            level.AddPoint(this);
        }
    }

    // Khi despawn/disable mà không có OnTriggerExit → vẫn trả chỗ
    private void OnBotDespawnedOrDisabled(Bot bot)
    {
        if (bot != currentBot) return;

        occupants = 0;
        DetachCurrentBot();
        level.AddPoint(this); // trả về FREE
    }

    private void AttachBot(Bot bot)
    {
        if (currentBot == bot) return;

        DetachCurrentBot();
        currentBot = bot;
        currentBot.BindSpawnPoint(this, level);

        // ĐĂNG KÝ sự kiện từ Bot
        currentBot.OnDespawned += OnBotDespawnedOrDisabled;
        currentBot.OnDisabled  += OnBotDespawnedOrDisabled;
    }

    private void DetachCurrentBot()
    {
        if (currentBot != null)
        {
            currentBot.OnDespawned -= OnBotDespawnedOrDisabled;
            currentBot.OnDisabled  -= OnBotDespawnedOrDisabled;
            currentBot = null;
        }
    }

    // Cho phép force từ Bot/Level
    public void ForceRelease()
    {
        occupants = 0;
        DetachCurrentBot();
        level.AddPoint(this);
    }
}
