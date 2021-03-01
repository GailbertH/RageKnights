using UnityEngine;

public class HealthbarHandler : MonoBehaviour
{
    //HealthBr
    [SerializeField] private Transform playerHealth;
    [SerializeField] private Transform enemyHealthBar;
    //Action Gauge
    [SerializeField] private Transform playerActionGauge;
    [SerializeField] private Transform enemyActionGauge;

    [SerializeField] private GameObject playerBarHolder;
    [SerializeField] private GameObject enemyBarHolder;

    private float playerBaseHealth;
    private float playerMaxActionGagugePoints;
    private float playerMaxRagePoints;
    private float enemyMaxActionGaugePoints;
    private float enemyBaseHealth;

    public void SetUIActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    public void Initialize(float playerBaseHealth, float enemyBaseHealth, float playerMaxActionPoints, float playerMaxRagePoints, float enemyMaxActionPoints, bool isBoss = false)
    {
        this.playerBaseHealth = playerBaseHealth;
        this.enemyBaseHealth = enemyBaseHealth;
        this.playerMaxActionGagugePoints = playerMaxActionPoints;
        this.playerMaxRagePoints = playerMaxRagePoints;
        this.enemyMaxActionGaugePoints = enemyMaxActionPoints;
    }

    public void ShowHealthBar()
    {
        playerBarHolder.SetActive(true);
        enemyBarHolder.SetActive(true);
    }

    public void HideHealthBar()
    {
        playerBarHolder.SetActive(false);
        enemyBarHolder.SetActive(false);
    }

    public void UpdatePlayerHealth(float playerCurrentHealth)
    {
        float healthPercent = playerCurrentHealth / playerBaseHealth;
        if (healthPercent < 0 || healthPercent > 1)
        {
            healthPercent = healthPercent < 0 ? 0 : 1;
        }
        playerHealth.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void UpdateEnemyHealth(float enemyCurrentHealth)
    {
        float healthPercent = (float)enemyCurrentHealth / (float)enemyBaseHealth;
        if (healthPercent < 0 || healthPercent > 1)
        {
            healthPercent = healthPercent < 0 ? 0 : 1;
        }
        else
        {
            enemyHealthBar.localScale = new Vector3(healthPercent, 1, 1);
        }
    }

    public void UpdatePlayerActionGauge(float playerCurrentActionGauge)
    {
        float actionGaugePercent = playerCurrentActionGauge / playerMaxActionGagugePoints;
        if (actionGaugePercent < 0 || actionGaugePercent > 1)
        {
            actionGaugePercent = actionGaugePercent < 0 ? 0 : 1;
        }
        playerActionGauge.localScale = new Vector3(actionGaugePercent, 1, 1);
    }

    public void UpdatePlayerMaxActionGauge(float current)
    {
        if(this.playerMaxActionGagugePoints != current)
            this.playerMaxActionGagugePoints = current;
    }
}
