using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> tempLocations;
    [SerializeField]
    private Transform enemyHandler;
    [SerializeField]
    private GameObject enemyCopy;

    private static EnvironmentManager instance;
    public static EnvironmentManager Instance { get { return instance; } }
    private Dictionary<string, EnemyAdventureData> enemies = new Dictionary<string, EnemyAdventureData>();

    private void Start()
    {
        if (RecordKeeperManager.Instance == null)
        {
            StartCoroutine(tempWaiter());
        }
        else
        {
            Initialize();
        }
    }
    private IEnumerator tempWaiter()
    {
        yield return new WaitUntil(() => RecordKeeperManager.Instance != null);
        Initialize();
        yield return new WaitForEndOfFrame();
    }
    private void Initialize()
    {
        if (RecordKeeperManager.Instance.curEnemyData == null)
        {
            GenerateEnemy();
        }
        else
        {
            enemies = RecordKeeperManager.Instance.curEnemyData;
        }
        PopulateEnvironment();
    }

    private void GenerateEnemy()
    {
        foreach (var loc in tempLocations)
        {
            EnemyAdventureData newEnemy = new EnemyAdventureData().GenerateTempData(loc);
            enemies.Add(newEnemy.adventureId, newEnemy);
        }
        RecordKeeperManager.Instance.curEnemyData = enemies;
    }

    private void PopulateEnvironment()
    {
        foreach (var enemy in enemies)
        {
            GameObject obj = Instantiate<GameObject>(enemyCopy, enemyHandler);
            obj.transform.localPosition = enemy.Value.location;
            obj.GetComponent<EnemyAdventureController>().adventureId = enemy.Value.adventureId;
            obj.SetActive(true);
        }
    }
}

public class EnemyAdventureData
{
    public string adventureId;
    public string unitId;
    public string locationId;
    public Vector3 location;

    public EnemyAdventureData GenerateTempData(Vector3 tempLocation)
    {
        return new EnemyAdventureData
        {
            adventureId = Guid.NewGuid().ToString(),
            unitId = "",
            locationId = "",
            location = tempLocation
        };
    }
}
