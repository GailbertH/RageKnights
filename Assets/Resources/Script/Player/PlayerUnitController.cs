using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : MonoBehaviour
{
    public const float HEAL_PERCENTAGE = 0.30f;

    private PlayerUnitModel unitData = null;
    private float actionGaugeModifier = 0;
    private bool isRageMode = false;

    public PlayerUnitModel GetUnitData
    {
        get { return unitData; }
    }

    public bool isActionGaugeFull
    {
        get { return unitData.MaxActionGaugePoints <= unitData.ActionGaugePoints; }
    }

    public void Init(PlayerUnitModel data)
    {
        unitData = data;
        ResetAnimation();
    }

    public void PlayerActionGauge(float incremnent)
    {
        //Debug.Log(currentPlayerData.MaxActionGaugePoints  + " <= " + currentPlayerData.ActionGaugePoints + " inc " + incremnent);
        if (unitData.ActionGaugePoints >= unitData.MaxActionGaugePoints)
        {
            if(isRageMode)
                unitData.ActionGaugePoints += incremnent * 2;
            else
                unitData.ActionGaugePoints += incremnent;
        }
    }



    //Animation


    public void PlayMoveAnimation()
    {
        PlayNormal("Forward");
    }

    public void PlayAttackAnimation()
    {
        //if (playerAnim == null)
        //    return;

        //if (IsAttackPlaying() == false)
        //{
        //    playerAnim["Attack"].speed = 5.0f;
        //    playerAnim.Play("Attack");
        //}
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
        //if (playerAnim == null)
        //    return;

        //if (playerAnim.isPlaying)
        //{
        //    playerAnim?.Stop();
        //}
        //playerAnim?.Play(animationName);
    }

    public bool IsAttackPlaying()
    {
        return false; //playerAnim.IsPlaying("Attack");
    }

    private void PlayNoRepeat(string animationName)
    {
        //if (playerAnim == null)
        //    return;

        //if (playerAnim.IsPlaying(animationName))
        //    return;

        //playerAnim.Play(animationName);
    }
}
