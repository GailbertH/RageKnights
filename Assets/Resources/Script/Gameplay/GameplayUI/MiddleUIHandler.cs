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
        PlayerUnitModel playerData = inst.PlayerHandler.GetPlayerData;

        float playerMaxHP = playerData.MaxHealthPoints;
        float playerCurrentHP = playerData.HealthPoints;
        float enemyCurrentHP = inst.EnemyHandler.GetArmyCount;

        healthbarHandler.Initialize(playerMaxHP, enemyCurrentHP, 150, enemyArmyCount: (int)enemyCurrentHP, isBoss: false);
        healthbarHandler.UpdatePlayerHealth(playerCurrentHP);
        healthbarHandler.UpdateEnemyHealth(enemyCurrentHP);
        healthbarHandler.UpdatePlayerActionGauge(0);
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
