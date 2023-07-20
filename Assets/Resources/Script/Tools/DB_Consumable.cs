﻿using RageKnight.Database;
using System;
using UnityEngine;
namespace RageKnight.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "Assets/Database_Item/Consumable")]
    public class DB_Consumable : DatabaseItem
    {
        public DB_Consumable()
        {
            id = "";
            name = "new item";
            description = "description";
            cost = 0;
            potency = 0;
            baseStockCount = 0;
            ItemEffectType = ItemEffectType.HEALING;
            icon = null;
        }

        public void PopulateData(DB_Consumable newCons)
        {
            id = newCons.id;
            name = newCons.name;
            description = newCons.description;
            cost = newCons.cost;
            potency = newCons.potency;
            baseStockCount = newCons.baseStockCount;
            ItemEffectType = newCons.ItemEffectType;
            icon = newCons.icon;
        }

        public int potency;
        public int baseStockCount;
        public ItemEffectType ItemEffectType;
    }
}