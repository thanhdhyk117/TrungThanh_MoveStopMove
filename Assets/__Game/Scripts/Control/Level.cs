using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int totalBotCount = 20;
    [SerializeField] private int botCountInTheGround = 10;
    [SerializeField] private Bot botPrefab;

    [Header("Runtime")]
    [SerializeField] private List<PointSpawn> listPosSpawns = new List<PointSpawn>(); // CHỈ chứa point rảnh
    [SerializeField] private List<Bot> bots = new List<Bot>();

    public int indexSpawn = 0;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        // Tự động nạp các PointSpawn và coi như rảnh ngay từ đầu
        listPosSpawns.Clear();
        var spawns = FindObjectsOfType<PointSpawn>();
        foreach (var spawn in spawns)
        {
            if (spawn != null && !listPosSpawns.Contains(spawn))
            {
                listPosSpawns.Add(spawn);
                spawn.Init(this); // gán level cho point
            }
        }

        SpawnBot();
    }

    private void SpawnBot()
    {
        if (totalBotCount <= 0 || botPrefab == null || listPosSpawns.Count == 0)
            return;

        if (totalBotCount < botCountInTheGround)
            botCountInTheGround = totalBotCount;

        int spawnCount = botCountInTheGround - bots.Count;
        while (spawnCount > 0 && bots.Count < botCountInTheGround && listPosSpawns.Count > 0)
        {
            // Lấy point RẢNH theo round-robin
            PointSpawn spawnPoint = listPosSpawns[indexSpawn % listPosSpawns.Count];
            indexSpawn++;

            // Khi sử dụng -> point trở thành BẬN
            RemovePoint(spawnPoint);

            var bot = SimplePool.Spawn<Bot>(botPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            bot.gameObject.SetActive(true);

            // bot biết point & level để phát sự kiện đúng lúc
            bot.BindSpawnPoint(spawnPoint, this);
            bots.Add(bot);
            bot.OnInit();

            totalBotCount--;
            spawnCount--;

            if (botCountInTheGround <= 0)
            {
                Debug.Log("Player win! Next level");
                break;
            }
        }
    }

    // ===== API quản lý điểm rảnh =====
    public void AddPoint(PointSpawn point)   // point trở lại RẢNH
    {
        if (point == null) return;
        if (!listPosSpawns.Contains(point))
            listPosSpawns.Add(point);
        // Debug.Log($"Point FREE: {point.name}");
    }

    public void RemovePoint(PointSpawn point) // point trở thành BẬN
    {
        if (point == null) return;
        listPosSpawns.Remove(point);
        // Debug.Log($"Point BUSY: {point.name}");
    }

    // ===== API quản lý bot =====
    public void DespawnBot(Bot bot)
    {
        if (bot == null) return;

        // YÊU CẦU bot tự xử lý trả chỗ + phát sự kiện
        bot.Despawn();

        bots.Remove(bot);
    }

#if UNITY_EDITOR
    [ContextMenu("GetListPosSpawn (Editor)")]
    private void GetListPosSpawn()
    {
        listPosSpawns.Clear();
        var spawns = FindObjectsOfType<PointSpawn>();
        foreach (var spawn in spawns)
        {
            if (spawn != null && !listPosSpawns.Contains(spawn))
            {
                listPosSpawns.Add(spawn);
            }
        }
        Debug.Log($"Found {listPosSpawns.Count} spawn points.");
    }
#endif
}
