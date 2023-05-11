using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyUnitModel
{
    public string name = "Unit";
    public string unitID = "TBA"; //Unit ID, unique to all units.
    public string unitCombatID; //This is a units ID that changes every combat.

    public int healthPoints;
    public int maxHealthPoints;

    public int manaPoints;
    public int maxManaPoints = 100;

    public int attackPower;
    public int defensePower;
    public int vitalityPower;

    public StatusBarFields statusBarItems
    {
        get
        {
            return new StatusBarFields
            {
                name = name,
                unitCombatID = unitCombatID,
                healthPoints = healthPoints,
                maxHealthPoints = maxHealthPoints,
                manaPoints = manaPoints,
                maxManaPoints = maxManaPoints,

                isEnemy = true
            };
        }
    }
}
