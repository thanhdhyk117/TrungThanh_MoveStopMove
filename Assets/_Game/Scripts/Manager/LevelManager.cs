using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;

    private List<Bot> bots = new List<Bot>();

    [SerializeField] Level[] levels;
    public Level currentLevel;

    private int totalBot;
    private bool isRevive;

    private int levelIndex;

    public int TotalCharater => totalBot + bots.Count + 1;

    public void Start()
    {
        levelIndex = 0;
        OnLoadLevel(levelIndex);
        OnInit();
    }

    public void OnInit()
    {
        player.OnInit();

        for (int i = 0; i < currentLevel.botInit; i++)
        {
            NewBot(null);
        }

        totalBot = currentLevel.botTotal - currentLevel.botInit - 1;

        isRevive = false;

        // SetTargetIndicatorAlpha(0);
    }

    public void OnReset()
    {
        // player.OnDespawn();
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].OnDespawn();
        }

        bots.Clear();
        SimplePool.CollectAll();
    }

    public void OnLoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levels[level]);
    }

    public Vector3 RandomPoint()
    {
        Vector3 randPoint = Vector3.zero;

        float size = Character.ATTACK_RANGE + Character.MAX_SIZE + 1f;

        for (int t = 0; t < 50; t++)
        {

            randPoint = currentLevel.RandomPoint();
            if (Vector3.Distance(randPoint, player.TF.position) < size)
            {
                continue;
            }

            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    if (Vector3.Distance(randPoint, bots[i].TF.position) < size)
                    {
                        break;
                    }
                }

                if (j == 19)
                {
                    return randPoint;
                }
            }


        }

        return randPoint;
    }

    private void NewBot(IState<Bot> state)
    {
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot, RandomPoint(), Quaternion.identity);
        bot.OnInit();
        bot.ChangeState(state);
        bots.Add(bot);

        bot.SetScore(player.Score > 0 ? Random.Range(player.Score - 7, player.Score + 7) : 1);
    }

    public void CharecterDeath(Character c)
    {
        if (c is Player)
        {
            UIManager.Ins.CloseAll();

            //revive
            if (!isRevive)
            {
                isRevive = true;
                UIManager.Ins.OpenUI<UIRevive>();
            }
            else
            {
                Fail();
            }
        }
        else
        if (c is Bot)
        {
            bots.Remove(c as Bot);

            if (GameManager.Ins.IsState(GameState.Revive) || GameManager.Ins.IsState(GameState.Setting))
            {
                NewBot(Utilities.Chance(50, 100) ? new IdleState() : new PatrolState());
            }
            else
            {
                if (totalBot > 0)
                {
                    totalBot--;
                    NewBot(Utilities.Chance(50, 100) ? new IdleState() : new PatrolState());
                }

                if (bots.Count == 0)
                {
                    Victory();
                }
            }

        }

        UIManager.Ins.GetUI<UIGameplay>().UpdateTotalCharacter();
    }

    private void Victory()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIVictory>().SetCoin(player.Coin);
        player.ChangeAnim(Constant.ANIM_WIN);
    }

    public void Fail()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIFail>().SetCoin(player.Coin);
    }

    public void Home()
    {
        UIManager.Ins.CloseAll();
        OnReset();
        OnLoadLevel(levelIndex);
        OnInit();
        UIManager.Ins.OpenUI<UIMainMenu>();
    }

    public void NextLevel()
    {
        levelIndex++;
    }

    public void OnPlay()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(new PatrolState());
        }
    }

    public void OnRevive()
    {
        player.TF.position = RandomPoint();
        player.OnRevive();
    }

    // public void SetTargetIndicatorAlpha(float alpha)
    // {
    //     List<GameUnit> list = SimplePool.GetAllUnitIsActive(PoolType.TargetIndicator);

    //     for (int i = 0; i < list.Count; i++)
    //     {
    //         (list[i] as TargetIndicator).SetAlpha(alpha);
    //     }
    // }
}
