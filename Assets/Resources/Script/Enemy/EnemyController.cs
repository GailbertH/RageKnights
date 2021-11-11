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
    [SerializeField] string deathAnimName;
    [SerializeField] string attackAnimName;

    [SerializeField] public bool testMode = false;

    private EnemyHandler enemyHandler = null;
    private bool enemyIsDead = false;

    private int enemyActionCounter = 0;

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

    public virtual void Initialize(CombatPlacement placementValue, EnemyHandler handler)
    {
        enemyHandler = handler;
        this.combatPlacement = placementValue;
        spriteRenderer.sortingOrder = GetOrderInLayer;
        Idle();
    }

    public virtual void CheckAction(RageKnight.Player.PlayerHandler pHandler)
    {
        enemyActionCounter += UnityEngine.Random.Range(1, 3);
        if (enemyActionCounter >= GetEnemyData.AttackCoolDownLength)
        {
            Attack();
            enemyActionCounter = 0;
            pHandler.PlayerDamaged(GetEnemyData.AttackPower);
        }
    }

    public virtual void Idle()
    {
        enemyAnimation.Play(idleAnimName);
    }

    public virtual bool Damaged(float damageReceive)
    {
        bool isAlive = true;
        if (enemyIsDead == true)
        {
            isAlive = false;
            return isAlive;
        }

        enemyAnimation.Play(damagedAnimName);
        GetEnemyData.HealthPoints -= damageReceive;
        if (GetEnemyData.HealthPoints <= 0)
        {
            Death();
            isAlive = false;
        }
        return isAlive;
    }

    public virtual void Death()
    {
        enemyHandler.DeductArmyCount();
        enemyAnimation.Play(deathAnimName);
        enemyIsDead = true;
        //Change this tp something that checks when enemy is dead then remove
        Invoke("DestroyEnemy", 0.5f);
    }

    public virtual void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    public virtual void Attack()
    {
        enemyAnimation.Play(attackAnimName);
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
