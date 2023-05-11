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
    [SerializeField] public UnitAnimationController unitAnimationController;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private CombatPlacement combatPlacement;

    private string unitCombatId;
    public string GetUnitCombatId
    {
        get { return unitCombatId; }
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

    private bool isTurnDone = false;
    public bool GetIsTurnDone
    {
        get { return isTurnDone; }
    }

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

    public virtual void Initialize(string unitCombatID)
    {
        spriteRenderer.sortingOrder = GetOrderInLayer;
        unitAnimationController.Idle();
        unitCombatId = unitCombatID;
    }

    public virtual void LoadUnit()
    {

    }

    public virtual void SetTarget()
    {

    }

    public virtual void Attack()
    {
        unitAnimationController.Attack();
        //handler.Damage(targetIds)
    }

    public virtual void Damaged()
    {

    }

    public virtual void Death()
    {
        unitAnimationController.Death();
        isDead = true;
        //DeductArmyCount ?
        Invoke("DestroyUnit", 0.5f);
    }

    public virtual void ResetAnimation()
    {
        unitAnimationController.ResetAnimation();
    }

    public virtual void DestroyUnit()
    {
        Destroy(this.gameObject);
    }

    public virtual void ShowOrHide(bool isShow = true)
    {
        this.gameObject.SetActive(isShow);
        if (isShow)
            unitAnimationController.Idle();
    }

    public void ResetTurn()
    {
        isTurnDone = false;
    }
    public void TurnEnd()
    {
        isTurnDone = true;
    }
}

public class PlayerUnitController : UnitController
{
    public const float HEAL_PERCENTAGE = 0.30f;

    private PlayerUnitModel unitData = null;
    private float actionGaugeModifier = 0;
    private bool isRageMode = false;

    public PlayerUnitModel UnitData
    {
        get { return unitData; }
        set { unitData = value; }
    }

    public override void Initialize(string unitCombatID)
    {
        base.Initialize(unitCombatID);
    }

    public void PlayMoveAnimation()
    {
        unitAnimationController.Idle();
    }
}
