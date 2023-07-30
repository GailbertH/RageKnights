using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordKeeperManager : MonoBehaviour
{
    private static RecordKeeperManager instance;
    public static RecordKeeperManager Instance { get { return instance; } }

    public Dictionary<string, EnemyAdventureData> curEnemyData;
    public string collideEnemyId;
    public Vector3 playerPosition;

    private void Awake()
    {
        instance = this;
        collideEnemyId = string.Empty;
        playerPosition = Vector3.zero;
        curEnemyData = null;
    }

    public void EnemyDefeated()
    {
        curEnemyData.Remove(collideEnemyId);
    }
}
