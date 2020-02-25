using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animation enemyAnimation;
    [SerializeField] private Transform enemyPosition;
    [SerializeField] private EnemyModel enemyData = null;
    private bool enemyIsDead = false;

    public EnemyModel GetEnemyData
    {
        get { return enemyData; }
    }

    public virtual Vector3 Position
    {
        get{ return enemyPosition.transform.position; }
    }

    public void Init()
    {
       
    }

    public virtual void Move(float moveLocation)
    {
        Vector3 newPosition = enemyPosition.transform.position;
        enemyPosition.transform.position = new Vector3((float)Math.Round(
            newPosition.x - moveLocation, 2),
            newPosition.y, newPosition.z);
    }

    public virtual void Initialize()
    {

    }

    public virtual void CheckAction()
    {

    }

    public virtual bool Damaged(int damageReceive)
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
}
