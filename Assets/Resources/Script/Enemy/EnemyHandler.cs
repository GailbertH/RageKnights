using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    //TODO too many random variables gotta reduce these weird variables its making things complicated now.
    private const float DEFAULT_ENEMY_ACTION_TIMER = 2f;
    public const float DEFAULT_SPAWN_TIMER = 5f;

    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private Animation spawnAnition;
    [SerializeField] private EnemySoldierController enemySoldierController;

    [SerializeField] private List<GameObject> spawnSpotHolders;

    private List<EnemyController> enemies = null;

    private int armyCount;
    private int enemyAttackDamage = 1;
    private float enemySpawnCD = 3f;
    private float enemyActionGauge = 0f;
    private float maxEnemyActionGauge = 170f;
    private bool hasPresentMonster = false;
    System.Random random = new System.Random();

    public bool HasSoldiers
    {
        get
        {
            return armyCount > 0;
        }
    }

    public float EnemySpawnCD
    {
        get
        {
            return enemySpawnCD;
        }
    }

    public int GetArmyCount
    {
        get
        {
            return armyCount;
        }
    }

    public void Initialize()
    {
        Debug.Log("init");
        //Not the best idea but it will do for now.
        armyCount = UnityEngine.Random.Range(20 , 50);
        enemies = new List<EnemyController>();
        enemies.Add(null);
        enemies.Add(null);
        enemies.Add(null);
        enemySpawnCD = DEFAULT_SPAWN_TIMER;
    }

    //Fix this to only get existing enemies
    public void DamagedEnemy(float damage)
    {
        var values = enemies.Where(x => x != null).Select(x => (int)x.GetCombatPlacement).ToList();

        if (values.Count <= 0)
            return;

        int eCPId = values[random.Next(values.Count)];

        string listContent = "";
        foreach (var x in values)
        {
            listContent += x + " ";
        }
        Debug.Log("Alive List : " + listContent);
        Debug.Log("Targe Enemy : " + eCPId);

        if (enemies[eCPId] != null)
        {
            bool isEnemyAlive = enemies[eCPId].Damaged(damage);
            if (isEnemyAlive == false)
            {
                OnEnemySlain(eCPId);
            }
        }
    }

    public void OnEnemySlain(int enemyPlacement)
    {
        armyCount--;
        Debug.Log("Enemy slain : " + enemyPlacement);
        UnSetEnemy(enemyPlacement);
        EnemySpawn();
        GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyArmyCount(armyCount);
        GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyHealth(armyCount);
    }

    public bool IsEnemySpawn()
    {
        Debug.Log("IsEnemySpawn");
        if (hasPresentMonster == true)
        {
            return true;
        }

        enemySpawnCD -= 1;
        float progressValue = (DEFAULT_SPAWN_TIMER - EnemySpawnCD) / DEFAULT_SPAWN_TIMER;
        GameManager.Instance.GameUIManager.ProgressbarHandler.UpdateStageProgress(progressValue);

        Debug.Log("IsEnemySpawn " + enemySpawnCD);
        if (enemySpawnCD <= 0)
        {
            Debug.Log(armyCount);
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
        else
        {
            enemySpawnCD = DEFAULT_SPAWN_TIMER;
            hasPresentMonster = false;
            return true;
        }
    }

    private void EnemySpawn()
    {
        var fieldSlots = enemies.Where(x => x != null).Count();
        Debug.Log("Army " + armyCount + " <= " + fieldSlots + " == " + spawnSpotHolders.Count());
        if (armyCount <= fieldSlots || fieldSlots == spawnSpotHolders.Count())
        {
            Debug.Log("Out of units");
            return;
        }

        Debug.Log("SPAWN");
        hasPresentMonster = true;
        int eCPId = enemies.FindIndex(x => x == null);

        var enemySpawnObj = spawnSpotHolders[eCPId];
        //need to reset everything for safety cause of animation
        GameObject enemyObject = Instantiate<GameObject>(enemyList[0],
            spawnSpotHolders[eCPId].transform.GetChild(0)
            ) as GameObject;

        enemyObject.transform.localPosition = enemyList[0].transform.position;
        enemyObject.transform.localRotation = enemyList[0].transform.rotation;

        enemies[eCPId] = enemyObject.GetComponent<EnemyController>();
        enemies[eCPId].Initialize((CombatPlacement)eCPId);
        enemySpawnObj.GetComponentInChildren<Animation>().Play();
        enemyObject.SetActive(true);
        enemySpawnCD = DEFAULT_SPAWN_TIMER;

        GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyHealth(enemies[eCPId].GetEnemyData.HealthPoints);

        if (fieldSlots < spawnSpotHolders.Count())
        {
            EnemySpawn();
        }
    }

    public void EnemyActionChecker(GameManager manager)
    {
        int eCPId = 0;
        enemyActionGauge += 1;
        if(enemyActionGauge >= maxEnemyActionGauge)
        {
            manager.PlayerHandler.PlayerDamaged(enemies[eCPId].GetEnemyData.AttackPower);
            enemyActionGauge = 0;
        }
    }

    public void UnSetEnemy(int enemyPlacement)
    {
        enemies[enemyPlacement]?.Death();
        enemies[enemyPlacement] = null;
    }

    public void UnSetAllEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i]?.DestroyEnemy();
            enemies[i] = null;
        }
        hasPresentMonster = false;
    }

    public void ShowOrHideEnemy(bool isShow = true)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ShowOrHide(isShow);
        }
    }


    //Rage Mode
    public void SetupRageMode()
    {
        enemySoldierController.Init(armyCount, enemyList);
        enemySoldierController.ShowUnits();
        ShowOrHideEnemy(false);
    }

    public void EndRageMode()
    {
        enemySoldierController.ShowUnits(false);
        enemySoldierController.End();
        ShowOrHideEnemy(true);
    }

    public void MoveUnits(float speed)
    {
        enemySoldierController.MoveUnits(speed);
    }

    public void DamageEnemyArmy(float damage)
    {
        enemySoldierController.DamageArmy(damage);
    }
}
