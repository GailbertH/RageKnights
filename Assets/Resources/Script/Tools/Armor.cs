using RageKnight.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Armor Item", menuName = "Assets/Database_Item/Armor")]
    public class Armor : DatabaseItem
    {
        public float defense;
        public float level;
        public List<float> defenseScalingPerLevel;
        public float maxLevel;
        public ElementType elementType;
    }
}
