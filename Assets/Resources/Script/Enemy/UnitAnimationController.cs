using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    [SerializeField] private Animation enemyAnimation;
    [SerializeField] private Animation extraEnemyAnimation;


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
        PlayExtraAnimation(damagedAnimName);
    }

    public virtual void Death()
    {
        PlayAnimation(deathAnimName);
    }

    //==========================================================
    private void PlayAnimation(string animationName)
    {
        enemyAnimation.Play(animationName);
    }
    private void PlayExtraAnimation(string animationName)
    {
        extraEnemyAnimation.Play(animationName);
    }
}
