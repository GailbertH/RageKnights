using RageKnight.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RageKnight.Database
{
    [CreateAssetMenu(fileName = "New Player Unit Container", menuName = "Assets/Database_Container/Unit")]
    public class DatabaseContainerUnit : DatabaseContainer
    {
        public List<DatabaseUnit> Units;
    }
}
