using RageKnight.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Weapon Item", menuName = "Assets/Database_Item/Weapon")]
    public class Weapon : DatabaseItem
    {
        public float damage;
        public float level;
        public List<float> damageScalingPerLevel;
        public float maxLevel;
        public ElementType elementType;
    }
}
