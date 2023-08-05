using RageKnight;
using RageKnight.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnitHandler : MonoBehaviour
{
    [SerializeField] private EnemySoldierController enemySoldierController;
    [SerializeField] private List<GameObject> spawnSpotHolders;
    [SerializeField] private List<EnemyUnitController> enemyUnits = null;
    private List<UnitModel> enemyUnitList = new List<UnitModel>(); //should be a queue

    public bool IsAlive
    {
        get
        {
            return enemyUnits.All(x => x.GetIsDead) == false;
        }
    }

    public List<UnitModel> GetCurrentEnemyData
    {
        get
        {
            return enemyUnits.Select(x => x.UnitData).ToList();
        }
    }

    public void EnemyInitialize(List<UnitModel> enemyDataList, int armyCount)
    {
        enemyUnitList = enemyDataList;
        //need to adjust with 1 - 3 units
        for (int i = 0; i < enemyUnitList.Count; i++)
        {
            Debug.Log("??? " + enemyUnitList[i].name);
            enemyUnits[i].Initialize(enemyUnitList[i]);
        }
        currentActiveUnit = enemyUnits[0];
    }

    /////////////////////////////////////////////////////
    EnemyUnitController currentActiveUnit = null;
    public string GetCurrentActiveUnitCombatId
    {
        get 
        {
            if (currentActiveUnit == null)
            {
                Debug.LogWarning("GGG - Inappropriate access");
                return String.Empty;
            }
            return currentActiveUnit.GetUnitCombatId;
        }
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
        currentActiveUnit = enemyUnits.FirstOrDefault
            (x => x.GetIsTurnDone == false && x.GetIsDead == false);
        if (currentActiveUnit != null)
        {
            isTurnsDone = false;
        }
        enemyTurnIsDone = isTurnsDone;
    }

    public void ResetTurns()
    {
        //All unit turnIsDone = false;
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            enemyUnits[i].ResetTurn();
        }
        currentActiveUnit = enemyUnits[0];
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

        foreach (string targetCombatID in targetCombatIDs)
        {
            DamageEnemyUnit(targetCombatID, damage);
        }
    }

    public void DamageEnemyUnit(string targetCombatID, int damageAmount)
    {
        if (enemyUnits.Count <= 0)
            return;

        var enemyUnit = enemyUnits.Find(x => x.GetUnitCombatId == targetCombatID);

        if (enemyUnit != null)
        {
            var currentHP = enemyUnit.DamageHealth(damageAmount);
            GameUIManager.Instance.HealthbarHandler.UpdateHealthPoints(targetCombatID, currentHP);

            bool isEnemyDead = enemyUnit.GetIsDead;
            if (isEnemyDead == true)
            {
                OnEnemySlain(enemyUnit);
            }
        }
    }

    public void OnEnemySlain(EnemyUnitController enemy)
    {
        Debug.Log("Enemy slain : " + enemy.GetUnitCombatId);
        //UnSetEnemy(enemy);
        //EnemySpawn();
    }

    /*
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
        int eCPId = enemies.FindIndex(x => x == null);

        var enemySpawnObj = spawnSpotHolders[eCPId];
        //need to reset everything for safety cause of animation
        GameObject enemyObject = Instantiate<GameObject>(enemyList[0],
            spawnSpotHolders[eCPId].transform
            ) as GameObject;

        enemyObject.transform.localPosition = enemyList[0].transform.position;
        enemyObject.transform.localRotation = enemyList[0].transform.rotation;

        enemies[eCPId] = enemyObject.GetComponent<EnemyUnitController>();


        enemies[eCPId].Initialize(enemyData);
        enemyObject.SetActive(true);

        //GameManager.Instance.GameUIManager.HealthbarHandler.UpdateEnemyHealth(enemies[eCPId].GetEnemyData.HealthPoints);

        if (fieldSlots < enemies.Count())
        {
            EnemySpawn();
        }
    }
    */

    public void EnemyActionChecker()
    {
        currentActiveUnit.CheckAction();
    }

    public CombatAction GetActionToExecute()
    {
        return currentActiveUnit.combatAction;
    }

    public void ExecuteAction()
    {
        currentActiveUnit.ExecuteAction();
    }
}
