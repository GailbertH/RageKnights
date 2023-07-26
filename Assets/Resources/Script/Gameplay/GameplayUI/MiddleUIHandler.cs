using RageKnight;
using System.Collections.Generic;
using System.Linq;
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
        progressHandler.SetUIActive(false);

        if (currentState == GameplayState.COMBAT)
        {
            InitHealthBar();
        }
    }

    private void InitHealthBar()
    {
        GameManager inst = GameManager.Instance;
        List<StatusBarFields> stats = new List<StatusBarFields>();
        List<StatusBarFields> enemyStats = new List<StatusBarFields>();

        stats = inst.PlayerHandler.GetPlayerData.Select(x => x.statusBarItems).ToList();
        enemyStats = inst.EnemyHandler.GetEnemyData.Select(x => x.statusBarItems).ToList();

        float enemyCurrentHP = inst.EnemyHandler.GetArmyCount;

        healthbarHandler.SetupPlayerSide(stats);
        healthbarHandler.SetupEnemySide(enemyStats);
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
