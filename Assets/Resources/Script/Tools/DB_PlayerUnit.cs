using RageKnight.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RageKnight.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "Assets/Database_Item/PlayerUnit")]
    public class DB_PlayerUnit : DatabaseUnit
    {
        public string role; //don't use this yet
        public string job; // dont use this yet

        public DB_PlayerUnit()
        {
            id = "";
            unitName = "new unit";
            healthPoints = 100;
            attackPower = 10;
            defensePower = 10;
            skillId1 = 0;
            skillId2 = 0;
            description = "description";
        }
    }

    public class DatabaseUnit : ScriptableObject
    {
        public string id;
        public string unitName;
        public float healthPoints;
        public int attackPower;
        public int defensePower;
        public int skillId1;
        public int skillId2;
        public Sprite icon;
        public Sprite splashArt;

        [TextArea]
        public string description;
    }
}


