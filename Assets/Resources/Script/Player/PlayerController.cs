using System.Collections;
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

    public PlayerModel GetPlayerData
    {
        get { return currentPlayerData; }
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

    public void Init()
    {
        currentPlayerData = new PlayerModel
        {
            AttackPower = 2,
            DefensePower = 2,
            HealthPower = 2,
            RagePower = 2,
            RageIncrement = 0.2f,
            ActionGaugeIncrement = 1f,
            HealthPoints = 10f,
            ActionGaugePoints = 10f,
            RagePoints = 0,
            ItemCount = 0,
            WeapomStatBonus = 0,
            HealthStatBonus = 0,
            ArmorStatBonus = 0,
            BaseHealthPoints = 10f,
            MaxRagePoints = 0,
            MaxActionGaugePoints = 100f
        };

        basePlayerData = currentPlayerData;
        ResetAnimation();
    }

    public void RevertBackToNormal()
    {
        currentPlayerData = basePlayerData;
    }

    public void RageModifier()
    {
        modifiedPlayerData = new PlayerModel
        {
            AttackPower = basePlayerData.AttackPower * 2,
            HealthPoints = 10,
            BaseHealthPoints = 10f,
            ItemCount = 0,
            RagePoints = 0,
        };

        currentPlayerData = modifiedPlayerData;
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
        if (currentPlayerData.HealthPoints > 0)
        {
            Debug.Log("Change Health to float someday");
            currentPlayerData.HealthPoints += (int)((float)currentPlayerData.BaseHealthPoints * HEAL_PERCENTAGE);
            if (currentPlayerData.HealthPoints > currentPlayerData.BaseHealthPoints)
            {
                currentPlayerData.HealthPoints = currentPlayerData.BaseHealthPoints;
            }
        }
    }

    public void PlayerActionGauge(float incremnent)
    {
        Debug.Log(currentPlayerData.MaxActionGaugePoints  + " <= " + currentPlayerData.ActionGaugePoints + " inc " + incremnent);
        if (CurrentMaxAGPoints > currentPlayerData.ActionGaugePoints)
        {
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
