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

    [SerializeField] Animation playerExtraAnim;
    [SerializeField] string damagedAnimName;

    public CompanionModel GetCompanionData
    {
        get { return companionData; }
    }

    public virtual void Initialize(CompanionModel statData)
    {
        //companionData = statData;
    }

    public virtual void CheckAction(EnemyUnitHandler eHandler)
    {
        companionActionCounter += UnityEngine.Random.Range(1, 3);
        if (companionActionCounter >= GetCompanionData.AttackCoolDownLength)
        {
            Attack();
            eHandler.DamagedEnemy(GetCompanionData.AttackPower, GameUIManager.Instance.GetTargets);
            companionActionCounter = 0;
        }
    }

    public virtual void Idle()
    { }

    public virtual void Damaged(float damageReceive)
    {
        playerExtraAnim.Play(damagedAnimName);
    }

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
