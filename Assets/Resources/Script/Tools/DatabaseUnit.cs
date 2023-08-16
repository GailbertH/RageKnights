using RageKnight.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RageKnight.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "Assets/Database_Item/Unit")]
    public class DatabaseUnit : ScriptableObject
    {
        public string id;
        public string unitName;
        public string role; //don't use this yet
        public string job; // dont use this yet
        public int baseHealthPoints; //base
        public int baseAttackPower; //base
        public int baseDefensePower; //base
        public Sprite icon;
        public Sprite splashArt;
        public GameObject unitPrefab;
        public AttackType normalAttackType = AttackType.MELEE;

        [TextArea]
        public string description;
    }

    public class DatabaseStatsInc
    {
        public int level;
        public int healthIncrement;
        public int attackIncrement;
        public int defenseIncrement;
    }
}


