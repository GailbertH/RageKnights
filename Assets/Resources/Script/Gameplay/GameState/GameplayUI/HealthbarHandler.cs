﻿using UnityEngine;

public class HealthbarHandler : MonoBehaviour
{
    //HealthBr
    [SerializeField] private Transform playerHealth;
    [SerializeField] private Transform enemyHealthBar;
    [SerializeField] private Transform bossHealthBar;
    //Action Gauge
    [SerializeField] private Transform playerActionGauge;
    [SerializeField] private Transform enemyActionGauge;
    [SerializeField] private Transform bossActionGauge;

    private float playerBaseHealth;
    private float playerMaxActionGagugePoints;
    private float playerMaxRagePoints;
    private float enemyMaxActionGaugePoints;
    private int enemyBaseHealth;
    private bool isBoss = false;

    public void SetUIActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    public void Initialize(float playerBaseHealth, int enemyBaseHealth, float playerMaxActionPoints, float playerMaxRagePoints, float enemyMaxActionPoints, bool isBoss = false)
    {
        this.playerBaseHealth = playerBaseHealth;
        this.enemyBaseHealth = enemyBaseHealth;
        this.playerMaxActionGagugePoints = playerMaxActionPoints;
        this.playerMaxRagePoints = playerMaxRagePoints;
        this.enemyMaxActionGaugePoints = enemyMaxActionPoints;
        this.isBoss = isBoss;
    }

    public void ShowHealthBar()
    {
        playerHealth.gameObject.SetActive(true);
        playerActionGauge.gameObject.SetActive(true);
        enemyHealthBar.gameObject.SetActive(!isBoss);
        enemyActionGauge.gameObject.SetActive(!isBoss);
        bossHealthBar.gameObject.SetActive(isBoss);
    }

    public void HideHealthBar()
    {
        playerHealth.gameObject.SetActive(false);
        playerActionGauge.gameObject.SetActive(false);
        enemyHealthBar.gameObject.SetActive(false);
        enemyActionGauge.gameObject.SetActive(false);
        bossHealthBar.gameObject.SetActive(false);
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

    public void UpdateEnemyHealth(int enemyCurrentHealth)
    {
        float healthPercent = (float)enemyCurrentHealth / (float)enemyBaseHealth;
        if (healthPercent < 0 || healthPercent > 1)
        {
            healthPercent = healthPercent < 0 ? 0 : 1;
        }
        if (isBoss)
        {
            bossHealthBar.localScale = new Vector3(healthPercent, 1, 1);
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
        Debug.Log(actionGaugePercent);
        playerActionGauge.localScale = new Vector3(actionGaugePercent, 1, 1);
    }


    public void UpdateEnemyActionGauge(float enemyCurrentActionGauge)
    {
        float actionGaugePercent = enemyCurrentActionGauge / enemyMaxActionGaugePoints;
        if (actionGaugePercent < 0 || actionGaugePercent > 1)
        {
            actionGaugePercent = actionGaugePercent < 0 ? 0 : 1;
        }
        enemyActionGauge.localScale = new Vector3(actionGaugePercent, 1, 1);
    }

    public void UpdatePlayerMaxActionGauge(float current)
    {
        if(this.playerMaxActionGagugePoints != current)
            this.playerMaxActionGagugePoints = current;
    }
}
