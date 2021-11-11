using RageKnight.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompController : MonoBehaviour
{
    private bool companionIsDead = false;

    private int companionActionCounter = 0;
    [SerializeField] private CompanionModel companionData;

    public CompanionModel GetCompanionData
    {
        get { return companionData; }
    }

    public virtual void Initialize(CompanionModel statData)
    {
        //companionData = statData;
    }

    public virtual void CheckAction(EnemyHandler eHandler)
    {
        companionActionCounter += UnityEngine.Random.Range(1, 3);
        if (companionActionCounter >= GetCompanionData.AttackCoolDownLength)
        {
            Attack();
            eHandler.DamagedEnemy(GetCompanionData.AttackPower);
            companionActionCounter = 0;
        }
    }

    public virtual void Idle()
    { }

    public virtual void Damaged(float damageReceive)
    { }

    public virtual void Death()
    { }

    public virtual void DestroyCompanion()
    { }

    public virtual void Attack()
    {
        //Attack animation
        Debug.Log(GetCompanionData.Name + " ATTACKS!");
    }
}
