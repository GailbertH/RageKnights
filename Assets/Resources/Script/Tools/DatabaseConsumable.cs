using RageKnight.Database;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.Database
{
    [CreateAssetMenu(fileName = "New Consumable Container", menuName = "Assets/Database_Container/Consumable")]
    public class DatabaseConsumable : DatabaseContainer
    {
        public List<Consumable> Consumables;
    }
}
