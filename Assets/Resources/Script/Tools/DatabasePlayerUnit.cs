using RageKnight.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RageKnight.Database
{
    [CreateAssetMenu(fileName = "New Player Unit Container", menuName = "Assets/Database_Container/Player_Unit")]
    public class DatabasePlayerUnit : DatabaseContainer
    {
        public List<DB_PlayerUnit> Units;
    }
}
