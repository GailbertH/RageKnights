using System;
using UnityEngine;

public class EnemyUnitController : UnitController
{
    private EnemyUnitModel unitData = null;

    public EnemyUnitModel UnitData
    {
        get { return unitData; }
        set { unitData = value; }
    }

    public override void Initialize(string unitCombatID)
    {
        base.Initialize(unitCombatID);
    }

    public void CheckAction()
    {
        Attack();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Damaged()
    {
        if (isDead == true)
        {
            return;
        }

        unitAnimationController.Damage();
        //GetEnemyData.HealthPoints -= 0;
        //if (GetEnemyData.HealthPoints <= 0)
        //{
        //    //Death();
        //    Debug.Log("=====DEAD!=====");
        //    isDead = false;
        //}
    }

    public override void Death()
    {
        base.Death();
        //enemyHandler.DeductArmyCount();
    }

}
