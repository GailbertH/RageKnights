using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationNames
{
    public const string IDLE = "Idle";
    public const string RUN = "Run";
    public const string ATTACK = "Attack";
    public const string HIT = "Hit";
    public const string DEATH = "Death";
}

public class UnitAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private Animation extraUnitAnimation;
    private Action animationCallback = null;
    private string currentState = string.Empty;
    public virtual void Idle()
    {
        ChangeAnimationState(UnitAnimationNames.IDLE);
    }

    public virtual void Attack(Action onAnimationEnd = null)
    {
        animationCallback = onAnimationEnd;
        ChangeAnimationState(UnitAnimationNames.ATTACK);
    }

    public virtual void Run()
    {
        ChangeAnimationState(UnitAnimationNames.RUN);
    }

    public virtual void Damage(Action onAnimationEnd = null)
    {
        animationCallback = onAnimationEnd;
        ChangeAnimationState(UnitAnimationNames.HIT);
    }

    public virtual void Death(Action onAnimationEnd = null)
    {
        animationCallback = onAnimationEnd;
        ChangeAnimationState(UnitAnimationNames.DEATH);
    }

    public virtual void ResetAnimation()
    {
        Idle();
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState && newState != UnitAnimationNames.HIT)
        {
            return;
        }

        unitAnimator.Play(newState);

        currentState = newState;
    }

    public void AnimationEnd()
    {
        if (animationCallback != null)
        {
            Debug.Log("Hello");
            animationCallback.Invoke();
            animationCallback = null;
        }
        ResetAnimation();
    }
}
