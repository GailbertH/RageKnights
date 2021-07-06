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
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] string idleAnimName;
    [SerializeField] string damagedAnimName;

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

    public int GetOrderInLayer
    {
        get {
            if (combatPlacement == CombatPlacement.BOT)
            {
                return 20;
            }
            else if (combatPlacement == CombatPlacement.MID)
            {
                return 10;
            }
            //Top returns 0
            return 0;
        }
    }

    public virtual void Initialize(CombatPlacement placementValue)
    {
        this.combatPlacement = placementValue;
        spriteRenderer.sortingOrder = GetOrderInLayer;
        Idle();
    }

    public virtual void CheckAction()
    {

    }

    public virtual void Idle()
    {
        enemyAnimation.Play(idleAnimName);
    }

    public virtual bool Damaged(float damageReceive)
    {
        bool isAlive = true;
        enemyAnimation.Play(damagedAnimName);
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

    public virtual void ShowOrHide(bool isShow = true)
    {
        this.gameObject.SetActive(isShow);
        if(isShow)
            Idle();
    }
}
