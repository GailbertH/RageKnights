using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animation enemyAnimation;
    [SerializeField] private EnemyModel enemyData = null;
    private bool enemyIsDead = false;

    public EnemyModel GetEnemyData
    {
        get { return enemyData; }
    }

    public void Init()
    {
       
    }

    public virtual void Initialize()
    {

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
