using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    [SerializeField] private Animation unitAnimation;
    [SerializeField] private Animation extraUnitAnimation;


    [SerializeField] string idleAnimName;
    [SerializeField] string damagedAnimName;
    [SerializeField] string deathAnimName;
    [SerializeField] string attackAnimName;

    public virtual void Idle()
    {
        PlayAnimation(idleAnimName);
    }

    public virtual void Attack()
    {
        PlayAnimation(attackAnimName);
    }

    public virtual void Damage()
    {
        if(extraUnitAnimation != null)
            PlayExtraAnimation(damagedAnimName);
        else
            PlayAnimation(damagedAnimName);
    }

    public virtual void Death()
    {
        PlayAnimationExclusive(deathAnimName);
    }

    public virtual void ResetAnimation()
    {
        Idle();
    }

    //==========================================================
    private void PlayAnimation(string animationName)
    {
        if (unitAnimation.isPlaying)
        {
            unitAnimation?.Stop();
        }
        unitAnimation.Play(animationName);
    }

    private void PlayAnimationExclusive(string animationName)
    {
        if (unitAnimation.IsPlaying(animationName))
            return;

        if (unitAnimation.isPlaying)
        {
            unitAnimation?.Stop();
        }

        unitAnimation.Play(animationName);
    }

    private void PlayExtraAnimation(string animationName)
    {
        extraUnitAnimation.Play(animationName);
    }
}
