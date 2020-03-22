using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private GameObject spawnSpot;
    public const float DEFAULT_SPAWN_TIMER = 5f;
    private const float DEFAULT_ENEMY_ACTION_TIMER = 2f;
    private float enemySpawnCD = 5f;
    private float enemyActionGauge = 0f;
    private float maxEnemyActionGauge = 170f;
    private int enemyAttackDamage = 1;
    private bool hasPresentMonster = false;
    private bool isEnemyAlive = true;
    private EnemyController enemy = null;

    public Transform GetEnemyTransform
    {
        get {
            if (enemy == null)
                return null;
            else
                return enemy.transform;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isEnemyAlive;
        }
    }

    public float EnemySpawnCD
    {
        get
        {
            return enemySpawnCD;
        }
    }

    public EnemyModel GetEnemyData
    {
        get
        {
            return enemy?.GetEnemyData;
        }
    }

    public void Initialize()
    {
        enemySpawnCD = DEFAULT_SPAWN_TIMER;
    }

    public void DamagedEnemy(float damage)
    {
        if (enemy != null)
        {
            isEnemyAlive = enemy.Damaged(damage);
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyHealth(enemy.GetEnemyData.HealthPoints);
        }
    }

    private void MoveEnemy(float speed)
    {
        enemy.Move(speed);
    }

    private bool IsEnemyInPosition
    {
        get { return (enemy.Position.x <= 2); }
    }

    public bool IsEnemySpawn()
    {
        if (hasPresentMonster == true)
        {
            return true;
        }

        enemySpawnCD -= 1;
        float progressValue = (DEFAULT_SPAWN_TIMER - EnemySpawnCD) /DEFAULT_SPAWN_TIMER;
        GameManager.Instance.GameUIManager.ProgressbarHandler.UpdateStageProgress(progressValue);

        if (enemySpawnCD <= 0)
        {
            EnemySpawn();
            return true;
        }
        return false;
    }

    public bool IsEnemyInBattlePosition(float enemyMovement, GameManager manager)
    {
        if (hasPresentMonster == false)
        {
            return false;
        }

        if (enemy != null)
        {
            if (IsEnemyInPosition == false)
            {
                MoveEnemy((enemyMovement * (-1)));
            }
            else
            {
                enemySpawnCD = DEFAULT_SPAWN_TIMER;
                hasPresentMonster = false;
                return true;
            }
        }
        return false;
    }

    private void EnemySpawn()
    {
        Debug.Log("SPAWN");
        isEnemyAlive = true;
        hasPresentMonster = true;
        GameObject enemyObject = Instantiate<GameObject>(enemyList[0],
            spawnSpot.transform.position,
            spawnSpot.transform.rotation, 
            this.transform
            ) as GameObject;
        enemy = enemyObject.GetComponent<EnemyController>();
        enemyObject.SetActive(true);
        enemySpawnCD = DEFAULT_SPAWN_TIMER;
    }

    public void EnemyActionChecker(GameManager manager)
    {
        enemyActionGauge += 1;
        if(enemyActionGauge >= maxEnemyActionGauge)
        {
            manager.PlayerHandler.PlayerDamaged(enemy.GetEnemyData.AttackPower);
            enemyActionGauge = 0;
        }
        manager.GameUIManager.MiddleUIHandler.HealthbarHandler.UpdateEnemyActionGauge(enemyActionGauge);
    }

    public void UnSetEnemy()
    {
        enemy.DestroyEnemy();
        enemy = null;
    }
}
