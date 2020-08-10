using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.Database
{
    public class DatabaseContainer : ScriptableObject
    {
        public string id;
    }

    [CreateAssetMenu(fileName = "New Consumable Container", menuName = "Assets/Database_Container/Consumable")]
    public class DatabaseConsumable : DatabaseContainer
    {
        public List<Consumable> Consumables;
    }
}
