using RageKnight;
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
        PlayerModel playerData = inst.PlayerHandler.GetPlayerData;

        float playerBaseHP = playerData.BaseHealthPoints;
        float playerCurrentHP = playerData.HealthPoints;
        float playerMaxAG = playerData.MaxActionGaugePoints;
        float playerMaxRage = playerData.MaxRagePoints;
        float enemyCurrentHP = inst.EnemyHandler.GetEnemyData.HealthPoints;

        healthbarHandler.Initialize(playerBaseHP, enemyCurrentHP, playerMaxAG, playerMaxRage, 150, false);
        healthbarHandler.UpdatePlayerHealth(playerCurrentHP);
        healthbarHandler.UpdateEnemyHealth(enemyCurrentHP);
        healthbarHandler.UpdatePlayerActionGauge(0);
        healthbarHandler.UpdateEnemyActionGauge(0);
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
