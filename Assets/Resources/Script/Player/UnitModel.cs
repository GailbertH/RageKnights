﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitModel
{
    public string name;
    public string unitID; //Unit ID, unique to all units.
    public string unitCombatID; //This is a units ID that changes every combat.
    public Sprite icon;
    public Sprite splashArt;
    public GameObject unitPrefab;

    public int attackPower;
    public int defensePower;

    public int healthPoints;
    public int maxHealthPoints = 100;
    public int manaPoints;
    public int maxManaPoints = 100;

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
                icon = icon,

                isEnemy = false
            };
        }
    }

    public CommandField commandField
    {
        get
        {
            return new CommandField
            {
                name = name,
                unitCombatID = unitCombatID,
                splashArt = splashArt
            };
        }
    }
}

[Serializable]
public class StatusBarFields
{
    public string name;
    public string unitCombatID;
    public int healthPoints;
    public int maxHealthPoints;

    public int manaPoints;
    public int maxManaPoints;

    public Sprite icon;

    public bool isEnemy;
}

[Serializable]
public class CommandField
{
    public string name;
    public string unitCombatID;
    public Sprite splashArt;
}

#region Unused
[Serializable]
public class ItemModel
{
    public string id; // REQUIRED
    public string name;
    public string description;
    public Sprite icon;
}

[Serializable]
public class ConsumableModel : ItemModel
{
    public int quantity; // REQUIRED
    public int potency;
    public ItemEffectType effectType;
}

[Serializable]
public class WeaponModel : ItemModel
{
    public float damage;
    public float level;
    public List<float> damageScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}

[Serializable]
public class HelmetModel : ItemModel
{
    public float health;
    public float level;
    public List<float> healthScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}

[Serializable]
public class ArmorModel : ItemModel
{
    public float defense;
    public float level;
    public List<float> defenseScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}
#endregion