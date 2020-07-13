using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    NONE = 0,
    FIRE = 1,
    WATER = 2,
    WIND = 3,
    EARTH = 4,
    LIGHT = 5,
    DARK = 6
}

public enum ItemEffectType
{
    HEALING = 0,
    DAMAGE_AMP = 1
}

public class DatabaseItem : ScriptableObject
{
    public string id;
    public new string name;
    [TextArea]
    public string description;
    public float cost;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Assets/Database_Item/Consumable")]
public class Consumable : DatabaseItem
{
    public int potency;
    public int baseStockCount;
    public ItemEffectType ItemEffectType;
}

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Assets/Database_Item/Weapon")]
public class Weapon : DatabaseItem
{
    public float damage;
    public float level;
    public List<float> damageScalingPerLevel;
    public float maxLevel;
    public ElementType elementType; 
}

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Assets/Database_Item/Armor")]
public class Armor : DatabaseItem
{
    public float defense;
    public float level;
    public List<float> defenseScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}

[CreateAssetMenu(fileName = "New Health Item", menuName = "Assets/Database_Item/health")]
public class Health : DatabaseItem
{
    public float health;
    public float level;
    public List<float> healthScalingPerLevel;
    public float maxLevel;
    public ElementType elementType;
}
