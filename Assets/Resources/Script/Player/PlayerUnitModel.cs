using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerUnitModel
{
    public float HealthPoints;
    public float MaxHealthPoints = 100;
    public float ActionGaugePoints;
    public float MaxActionGaugePoints = 100;
    public float RagePoints;
    public float MaxRagePoints = 100;

    public float RageIncrement;
    public float ActionGaugeIncrement;

    public int AttackPower;
    public int DefensePower;
    public int HealthPower;
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