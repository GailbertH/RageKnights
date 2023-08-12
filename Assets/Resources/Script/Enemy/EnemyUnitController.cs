using System;
using UnityEngine;

public class EnemyUnitController : UnitController
{
    public EnemyUnitController(UnitModel unitData) : base(unitData)
    {
    }

    public void CheckAction()
    {
        combatAction = CombatAction.ATTACK;
    }
}
