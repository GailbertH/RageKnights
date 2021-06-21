using System;
using UnityEngine;

public enum CombatPlacement
{
    MID = 0,
    TOP = 1,
    BOT = 2
}
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animation enemyAnimation;
    [SerializeField] private EnemyModel enemyData = null;
    [SerializeField] private CombatPlacement combatPlacement;

    private bool enemyIsDead = false;

    public EnemyModel GetEnemyData
    {
        get { return enemyData; }
    }

    //TODO fix this someday
    public CombatPlacement GetCombatPlacement
    {
        get { return combatPlacement; }
    }

    public virtual void Initialize(CombatPlacement placementValue)
    {
        this.enemyAnimation = enemyAnimation;
        this.combatPlacement = placementValue;
    }

    public virtual void CheckAction()
    {

    }

    public virtual bool Damaged(float damageReceive)
    {
        bool isAlive = true;
        enemyData.HealthPoints -= damageReceive;
        if (enemyData.HealthPoints <= 0)
        {
            Death();
            isAlive = false;
        }
        return isAlive;
    }

    public virtual void Death()
    {
        enemyIsDead = true;
    }

    public virtual void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    public virtual void Attack()
    {

    }

    public virtual void LoadEnemy()
    {

    }
}
