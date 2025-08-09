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

    public int indexSpawn = 0;

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
            return;

        if (totalBotCount < botCountInTheGround)
            botCountInTheGround = totalBotCount;

        int spawnCount = botCountInTheGround - bots.Count;
        while (spawnCount > 0 && bots.Count < botCountInTheGround)
        {
            Transform spawnPoint = listPosSpawns[indexSpawn % listPosSpawns.Count].transform;
            indexSpawn++;

            var bot = SimplePool.Spawn<Bot>(botPrefab, spawnPoint.position, spawnPoint.rotation);
            bot.gameObject.SetActive(true);
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


#if UNITY_EDITOR

    [ContextMenu("GetListPosSpawn")]
    private void GetListPosSpawn()
    {
        listPosSpawns.Clear();
        var spawns = FindObjectsOfType<PointSpawn>();
        foreach (var spawn in spawns)
        {
            if (spawn != null && !listPosSpawns.Contains(spawn) && !spawn.isHaveCharacter)
            {
                listPosSpawns.Add(spawn);
            }
        }
        Debug.Log($"Found {listPosSpawns.Count} spawn points.");
    }
#endif


    public void AddPoint(PointSpawn point)
    {
        Debug.Log($"Adding point: {point.name}");
        if (point != null && !listPosSpawns.Contains(point))
        {
            listPosSpawns.Add(point);
            OnChangePoint += RemovePoint;
            Debug.Log($"Point added: {point.name}");
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
