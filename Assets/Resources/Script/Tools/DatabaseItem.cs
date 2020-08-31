using System;
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

namespace RageKnight.Database
{
    public class DatabaseItem : ScriptableObject
    {
        public string id;
        public new string name;
        [TextArea]
        public string description;
        public long cost;
        public Sprite icon;
    }
}
