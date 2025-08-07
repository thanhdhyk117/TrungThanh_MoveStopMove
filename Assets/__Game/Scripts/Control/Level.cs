using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int totalBotCount = 20;
    [SerializeField] private int botCountInTheGround = 10;
    [SerializeField] private Bot botPrefab;
    [SerializeField] private List<PointSpawn> listPosSpawns;
    //private List<PointSpawn> listPointSpawns = new List<PointSpawn>();
    [SerializeField] private List<Bot> bots = new List<Bot>();

    public Action<PointSpawn> OnChangePoint;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        SpawnBot();
    }

    private void SpawnBot()
    {
        if (totalBotCount <= 0 || botPrefab == null || listPosSpawns.Count == 0)
        {
            return;
        }

        if (totalBotCount < botCountInTheGround)
        {
            botCountInTheGround = totalBotCount;
        }

        int spawnCount = botCountInTheGround - bots.Count;

        for (int i = 0; i < spawnCount; i++)
        {
            if (bots.Count >= totalBotCount) break;
            Transform spawnPoint = listPosSpawns[UnityEngine.Random.Range(0, listPosSpawns.Count)].transform;

            var bot = SimplePool.Spawn<Bot>(botPrefab, spawnPoint.position, spawnPoint.rotation);
            bot.gameObject.SetActive(true);
            bots.Add(bot);
            bot.OnInit();
        }
    }

    [ContextMenu("GetListPosSpawn")]
    private void GetListPosSpawn()
    {
        listPosSpawns.Clear();
        var spawns = FindObjectsOfType<PointSpawn>();
        foreach (var spawn in spawns)
        {
            if (spawn != null && !listPosSpawns.Contains(spawn) && spawn.isHaveCharacter)
            {
                listPosSpawns.Add(spawn);
            }
        }
        Debug.Log($"Found {listPosSpawns.Count} spawn points.");
    }

    public void AddPoint(PointSpawn point)
    {
        if (point != null && !listPosSpawns.Contains(point))
        {
            listPosSpawns.Add(point);
            OnChangePoint += RemovePoint;
        }
    }

    public void RemovePoint(PointSpawn point)
    {
        if (point != null && listPosSpawns.Contains(point))
        {
            listPosSpawns.Remove(point);
            OnChangePoint -= RemovePoint;
        }
    }
}
