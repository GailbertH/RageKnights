﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const float HEAL_PERCENTAGE = 0.30f;

    [SerializeField] private Animation playerAnim;
    private PlayerModel currentPlayerData = null;
    private PlayerModel modifiedPlayerData = null;
    private PlayerModel basePlayerData = null;
    private float actionGaugeModifier = 0;
    private bool isRageMode = false;

    public PlayerModel GetPlayerData
    {
        get { return currentPlayerData; }
    }

    public float TotalAttackDamage
    {
        get
        {
            if (isRageMode)
                return currentPlayerData.AttackPower * currentPlayerData.AttackRageMultiplier;
            else
                return currentPlayerData.AttackPower;
        }
    }

    public bool isActionGaugeFull
    {
        get { return CurrentMaxAGPoints <= currentPlayerData.ActionGaugePoints; }
    }

    public float CurrentMaxAGPoints
    {
        get { return (currentPlayerData.MaxActionGaugePoints - actionGaugeModifier); }
    }

    public float ActionGagugeModifier
    {
        get { return actionGaugeModifier; }
        set { actionGaugeModifier = value; }
    }

    public int ItemCount
    {
        get { return currentPlayerData.ItemCount; }
    }

    public void Init(PlayerModel playerData)
    {
        currentPlayerData = playerData;
        ResetAnimation();
    }

    public void RevertBackToNormal()
    {
        isRageMode = false;
        Debug.Log("NORMAL!!");
    }

    public void RageModifier()
    {
        isRageMode = true;
        Debug.Log("RAGE!!");
    }

    public void PlayerDamaged(int damage)
    {
        currentPlayerData.HealthPoints -= damage;
        if (currentPlayerData.HealthPoints <= 0)
        {
            PlayDeathAnimation();
        }
    }

    public void PlayerHeal()
    {
        if (currentPlayerData.ItemCount > 0 && currentPlayerData.HealthPoints > 0)
        {
            currentPlayerData.ItemCount--;
            currentPlayerData.currentItemInUse.quantity--;
            currentPlayerData.HealthPoints += (int)((float)currentPlayerData.BaseHealthPoints * HEAL_PERCENTAGE);
            if (currentPlayerData.HealthPoints > currentPlayerData.BaseHealthPoints)
            {
                currentPlayerData.HealthPoints = currentPlayerData.BaseHealthPoints;
            }
            Debug.Log(currentPlayerData.ItemCount);
        }
    }

    public void PlayerAddItem(int addItemCount)
    {
        int curMaxItemCount = currentPlayerData.MaxItemCount;
        int newCurItemCount = currentPlayerData.ItemCount + addItemCount;
        currentPlayerData.currentItemInUse.quantity += addItemCount;
        currentPlayerData.ItemCount = newCurItemCount > curMaxItemCount ? curMaxItemCount : newCurItemCount;
    }

    public void PlayerActionGauge(float incremnent)
    {
        //Debug.Log(currentPlayerData.MaxActionGaugePoints  + " <= " + currentPlayerData.ActionGaugePoints + " inc " + incremnent);
        if (CurrentMaxAGPoints > currentPlayerData.ActionGaugePoints)
        {
            if(isRageMode)
                currentPlayerData.ActionGaugePoints += incremnent * 2;
            else
                currentPlayerData.ActionGaugePoints += incremnent;
        }
    }

    public void ResetActionGauge()
    {
        currentPlayerData.ActionGaugePoints = 0;
    }

    public void PlayerRagePoints(float incremnent)
    {
        if (currentPlayerData.MaxRagePoints > currentPlayerData.RagePoints)
        {
            currentPlayerData.RagePoints += incremnent;
        }
    }

    public void PlayerUseRageMode()
    {
        Debug.Log(currentPlayerData.RagePoints + " >= " + currentPlayerData.MaxRagePoints);
        if (currentPlayerData.RagePoints >= currentPlayerData.MaxRagePoints)
        {
            currentPlayerData.RagePoints = 0;
            RageModifier();
        }
    }

    public void PlayMoveAnimation()
    {
        PlayNormal("Forward");
    }

    public void PlayAttackAnimation()
    {
        PlayNormal("Attack");
    }

    public void PlayDeathAnimation()
    {
        PlayNoRepeat("Death");
    }

    public void ResetAnimation()
    {
        PlayNormal("Idle");
    }

    private void PlayNormal(string animationName)
    {
        if (playerAnim == null)
            return;

        if (playerAnim.isPlaying)
            playerAnim.Stop();

        playerAnim.Play(animationName);
    }

    private void PlayNoRepeat(string animationName)
    {
        if (playerAnim == null)
            return;

        if (playerAnim.IsPlaying(animationName))
            return;

        playerAnim.Play(animationName);
    }
}
