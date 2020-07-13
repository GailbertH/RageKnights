using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    public DatabaseConsumable dbConsumable;

    private static DatabaseManager instance;
    public static DatabaseManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

    public List<Consumable> GetAllConsumables()
    {
        return dbConsumable.Consumables;
    }

}
