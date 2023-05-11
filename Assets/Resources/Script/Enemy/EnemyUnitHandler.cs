using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnitHandler : MonoBehaviour
{
    //TODO too many random variables gotta reduce these weird variables its making things complicated now.
    private const float DEFAULT_ENEMY_ACTION_TIMER = 2f;
    public const float DEFAULT_SPAWN_TIMER = 5f;

    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private EnemySoldierController enemySoldierController;

    [SerializeField] private List<GameObject> spawnSpotHolders;
    [SerializeField] private int spawnLimit = 0;

    private List<EnemyUnitController> enemies = null;
    private int combatIdCounter = 0;

    private int armyCount;
    private int enemyAttackDamage = 1;
    private float enemySpawnCD = 3f;
    private bool hasPresentMonster = false;
    System.Random random = new System.Random();

    public int GetEnemyCount
    {
        get
        {
            return spawnLimit;
        }
    }

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

    public List<EnemyUnitModel> GetEnemyData
    {
        get
        {
            return enemies.Select(x => x.UnitData).ToList();
        }
    }

    public void Initialize()
    {
        Debug.Log("init");
        //Not the best idea but it will do for now.
        armyCount = UnityEngine.Random.Range(20, 50);
        spawnLimit = (spawnLimit == 0 || spawnLimit > 3) ? spawnSpotHolders.Count : spawnLimit;
        enemies = new List<EnemyUnitController>();
        for(int i = 0; i < spawnLimit; i++)
        {
            enemies.Add(null);
        }
        enemySpawnCD = DEFAULT_SPAWN_TIMER;
        currentActiveUnit = enemies[0];
    }

    /////////////////////////////////////////////////////
    EnemyUnitController currentActiveUnit = null;
    public EnemyUnitController GetCurrentActiveUnit
    {
        get { return currentActiveUnit; }
    }

    private bool enemyTurnIsDone = false;
    public void SetTurnOrder(bool isPlayerGoesFirst = false)
    {
        enemyTurnIsDone = isPlayerGoesFirst ? true : false;
    }

    public bool IsTurnsDone()
    {
        //All units done doing their turn
        return enemyTurnIsDone;
    }


    public void TurnEnd()
    {
        currentActiveUnit.TurnEnd();
    }

    public void UpdateTurns()
    {
        bool isTurnsDone = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetIsTurnDone == false)
            {
                Debug.Log("E Unit " + (i + 1) + " = " + enemies[i].GetIsTurnDone);
                currentActiveUnit = enemies[i];
                isTurnsDone = false;
            }
        }
        enemyTurnIsDone = isTurnsDone;

        Debug.Log("enemyTurnIsDone " + enemyTurnIsDone);
    }

    public void ResetTurns()
    {
        //All unit turnIsDone = false;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ResetTurn();
        }
        currentActiveUnit = enemies[0];
        enemyTurnIsDone = false;
    }

    public void CheckUnitAction()
    {

    }

    /////////////////////////////////////////////////////
    //Fix this to only get existing enemies
    public void DamagedEnemy(int damage, List<string> targetCombatIDs)
    {
        if (targetCombatIDs.Count <= 0)
            return;

        for (int i = 0; i < targetCombatIDs.Count; i++)
        {
            DamagedEnemy(damage, targetCombatIDs[i]);
        }
    }

    public void DamagedEnemy(int damage, string targetCombatID)
    {
        if (enemies.Count <= 0)
            return;

        var enemy = enemies.Find(x => x.GetUnitCombatId == targetCombatID);

        if (enemy != null)
        {
            enemy.Damaged();
            bool isEnemyDead = enemy.GetIsDead;
            if (isEnemyDead == true)
            {
                OnEnemySlain(enemy);
            }
        }
    }

    public void OnEnemySlain(EnemyUnitController enemy)
    {
        Debug.Log("Enemy slain : " + enemy.GetUnitCombatId);
        UnSetEnemy(enemy);
        EnemySpawn();
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
        GameUIManager.Instance.ProgressbarHandler.UpdateStageProgress(progressValue);

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
        Debug.Log("Army " + armyCount + " <= " + fieldSlots + " == " + enemies.Count());
        if (armyCount <= fieldSlots || fieldSlots == enemies.Count())
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

        enemies[eCPId] = enemyObject.GetComponent<EnemyUnitController>();
        EnemyUnitModel enemyData = new EnemyUnitModel
        {
            name = "blompy" + combatIdCounter,
            unitCombatID = "e" + combatIdCounter,
            healthPoints = 100,
            maxHealthPoints = 100,

            attackPower = 2,
            defensePower = 2,
            vitalityPower = 2
        };

        enemies[eCPId].UnitData = enemyData;
        enemies[eCPId].Initialize("e" + combatIdCounter);
        ++combatIdCounter;
        enemyObject.SetActive(true);
        enemySpawnCD = DEFAULT_SPAWN_TIMER;

        //GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyHealth(enemies[eCPId].GetEnemyData.HealthPoints);

        if (fieldSlots < enemies.Count())
        {
            EnemySpawn();
        }
    }

    public void EnemyActionChecker()
    {
        currentActiveUnit.CheckAction();
    }

    public void DeductArmyCount()
    {
        armyCount--;
        GameManager.Instance.EnemyKill();
        //GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyArmyCount(armyCount);
        //GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyHealth(armyCount);
    }

    public void UnSetEnemy(EnemyUnitController enemy)
    {
        enemy?.Death();
        enemies.Remove(enemy);
    }

    public void UnSetAllEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i]?.DestroyUnit();
            enemies[i] = null;
        }
        hasPresentMonster = false;
    }

    public void SetAllEnemy()
    {
        if(armyCount > 0)
        {
            EnemySpawn();
        }
    }


    //Rage Mode
    public void SetupRageMode()
    {
        enemySoldierController.Init(armyCount, enemyList);
        enemySoldierController.ShowUnits();
        UnSetAllEnemy();
    }

    public void EndRageMode()
    {
        enemySoldierController.ShowUnits(false);
        enemySoldierController.End();
        SetAllEnemy();
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
