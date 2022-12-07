using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatPlacement
{
    MID = 0,
    TOP = 1,
    BOT = 2
}
public class UnitController : MonoBehaviour
{
    [SerializeField] public UnitAnimationController animationController;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private CombatPlacement combatPlacement;
    protected List<string> targetIds;

    private string unitId;
    public string GetUnitId
    {
        get { return unitId; }
    }

    protected bool isDead = false;
    public bool GetIsDead
    {
        get { return isDead; }
    }

    public CombatPlacement GetCombatPlacement
    {
        get { return combatPlacement; }
    }

    public bool IsTurnDone { get; set; }

    public int GetOrderInLayer
    {
        get
        {
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

    public virtual void Initialize()
    {
        spriteRenderer.sortingOrder = GetOrderInLayer;
        animationController.Idle();
        unitId = "E1";
    }

    public virtual void LoadUnit()
    {

    }

    public virtual void SetTarget()
    {

    }

    public virtual void Attack()
    {
        animationController.Attack();
        //handler.Damage(targetIds)
    }

    public virtual void Damaged()
    {

    }

    public virtual void ResetAnimation()
    {

    }

    public virtual void Death()
    {
        animationController.Death();
        isDead = true;
        //DeductArmyCount ?
        Invoke("DestroyUnit", 0.5f);
    }

    public virtual void DestroyUnit()
    {
        Destroy(this.gameObject);
    }

    public virtual void ShowOrHide(bool isShow = true)
    {
        this.gameObject.SetActive(isShow);
        if (isShow)
            animationController.Idle();
    }
}

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
