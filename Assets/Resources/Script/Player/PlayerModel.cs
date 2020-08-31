using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public float HealthPoints { get; set; }
    public float RagePoints { get; set; }
    public float ActionGaugePoints { get; set; }

    public float RageIncrement { get; set; }
    public float ActionGaugeIncrement { get; set; }

    public int AttackPower { get; set; }
    public int DefensePower { get; set; }
    public int HealthPower { get; set; }
    public int RagePower { get; set; }

    public ConsumableModel currentItemInUse { get; set; }
    public WeaponModel currentWeaponInUse { get; set; }
    public HelmetModel currentHelmetInUse { get; set; }
    public ArmorModel currentArmorInUse { get; set; }
    public int ItemCount { get; set; }

    public int WeapomStatBonus { get; set; } //ATTACK
    public int HealthStatBonus { get; set; } //DEFENSE
    public int ArmorStatBonus { get; set; } //HEALTH

    public float AttackRageMultiplier { get; set; }

    //Must be moved somewhere
    public float BaseHealthPoints { get; set; }
    public float MaxRagePoints { get; set; }
    public float MaxActionGaugePoints { get; set; }
    public int MaxItemCount { get; set; }
}

public class ItemModel
{
    public string id { get; set; } // REQUIRED
    public string name { get; set; }
    public string description { get; set; }
    public Sprite icon;
}

public class ConsumableModel : ItemModel
{
    public int quantity { get; set; } // REQUIRED
    public int potency { get; set; }
    public ItemEffectType effectType { get; set; }
}

public class WeaponModel : ItemModel
{
    public float damage;
    public float level;
    public List<float> damageScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}

public class HelmetModel : ItemModel
{
    public float health;
    public float level;
    public List<float> healthScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}

public class ArmorModel : ItemModel
{
    public float defense;
    public float level;
    public List<float> defenseScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}