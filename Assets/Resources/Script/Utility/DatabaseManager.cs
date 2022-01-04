using System.Collections;
using RageKnight.Database;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    public DatabaseConsumable dbConsumable;
    [SerializeField] private int frameRate = 60;
    private static DatabaseManager instance;
    public static DatabaseManager Instance { get { return instance; } }

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;

        instance = this;
    }

    public List<Consumable> GetAllConsumables()
    {
        return dbConsumable.Consumables;
    }

}
