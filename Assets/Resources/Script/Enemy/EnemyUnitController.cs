using System;
using UnityEngine;

public class EnemyUnitController : UnitController
{
    private EnemyUnitModel unitData = null;

    public EnemyUnitController(UnitModel unitData) : base(unitData)
    {
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
