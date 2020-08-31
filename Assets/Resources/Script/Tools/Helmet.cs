using RageKnight.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.Database
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Helmet Item", menuName = "Assets/Database_Item/helmet")]
    public class Helmet : DatabaseItem
    {
        public float health;
        public float level;
        public List<float> healthScalingPerLevel;
        public float maxLevel;
        public ElementType elementType;
    }
}
