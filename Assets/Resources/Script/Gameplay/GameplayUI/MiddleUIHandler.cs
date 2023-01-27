using RageKnight;
using System.Collections.Generic;
using UnityEngine;

public class MiddleUIHandler : MonoBehaviour
{
    [SerializeField] private HealthbarHandler healthbarHandler;
    [SerializeField] private ProgressBarHandler progressHandler;

    public void Initialize()
    {
        healthbarHandler.SetUIActive(false);
        progressHandler.SetUIActive(false);
        progressHandler.Initialize();
    }

    public void SetMiddleGround(GameplayState currentState)
    {
        healthbarHandler.SetUIActive(currentState == GameplayState.COMBAT);
        progressHandler.SetUIActive(currentState == GameplayState.ADVENTURE);

        if (currentState == GameplayState.COMBAT)
        {
            InitHealthBar();
        }
    }

    private void InitHealthBar()
    {
        GameManager inst = GameManager.Instance;
        PlayerUnitModel playerData = inst.PlayerHandler.GetPlayerData;
        List<StatusBarFields> stats = new List<StatusBarFields>();

        StatusBarFields stat = playerData.statusBarItems;
        stat.name = "Unit 1";
        stats.Add(stat);

        stat = playerData.statusBarItems;
        stat.name = "Unit 2";
        stats.Add(stat);

        stat = playerData.statusBarItems;
        stat.name = "Unit 3";
        stats.Add(stat);

        float playerMaxHP = playerData.maxHealthPoints;
        float playerCurrentHP = playerData.healthPoints;
        float enemyCurrentHP = inst.EnemyHandler.GetArmyCount;

        healthbarHandler.SetupPlayerSide(stats);
        healthbarHandler.SetupEnemySide(2);
        healthbarHandler.ShowHealthBar();
    }

    public HealthbarHandler HealthbarHandler
    {
        get { return healthbarHandler; }
    }
    public ProgressBarHandler ProgressbarHandler
    {
        get { return progressHandler; }
    }
}
