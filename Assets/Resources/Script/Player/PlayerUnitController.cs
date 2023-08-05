using RageKnight;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;

public enum CombatPlacement
{
    MID = 0,
    TOP = 1,
    BOT = 2
}

public class UnitController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] public UnitAnimationController unitAnimationController;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private CombatPlacement combatPlacement;
    [SerializeField] private GameObject targetMarker = null;
    public CombatAction combatAction = CombatAction.NONE;
    public string targetCombatId = "";

    public UnitController(UnitModel unitData)
    {
        //UnitData = unitData;
    }
    public UnitModel UnitData{ get; private set;}

    public string GetUnitCombatId
    {
        get { return UnitData.unitCombatID; }
    }

    public bool GetIsDead
    {
        get { return UnitData.healthPoints <= 0; }
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

    public virtual void Initialize(UnitModel unitData)
    {
        UnitData = unitData;
        if (UnitData.unitPrefab != null)
        {
            GameObject unitObject = Instantiate<GameObject>(UnitData.unitPrefab,transform) as GameObject;
            unitAnimationController = unitObject.GetComponent<UnitAnimationController>();
            spriteRenderer = unitObject.GetComponent<SpriteRenderer>();

            spriteRenderer.sortingOrder = GetOrderInLayer;
            unitAnimationController.Idle();
        }
        GameTargetingManager.Instance.OnUnitTargetChange(this.TargetAim);
    }

    public virtual void LoadUnit()
    {

    }

    public virtual void SetTarget()
    {

    }

    public virtual void ExecuteAction()
    {
        if (combatAction == CombatAction.ATTACK)
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        unitAnimationController.Attack();
    }

    public virtual int DamageHealth(int damageAmount)
    {
        UnitData.healthPoints -= damageAmount;
        if (UnitData.healthPoints > 0)
        {
            Damage();
        }
        else
        {
            Death();
        }

        return UnitData.healthPoints;
    }

    public virtual void Damage()
    {
        unitAnimationController.Damage();
    }

    public virtual void Death()
    {
        unitAnimationController.Death();
    }

    public virtual void ResetAnimation()
    {
        unitAnimationController.ResetAnimation();
    }

    public virtual void DestroyUnit()
    {
        GameTargetingManager.Instance.RemoveOnUnitTargetChange(this.TargetAim);
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

    private void TargetAim(string isTarget)
    {
        if(targetMarker != null)
        targetMarker?.SetActive(GetUnitCombatId == isTarget);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameTargetingManager.Instance.TargetChange(GetUnitCombatId);
        }
    }
}

public class PlayerUnitController : UnitController
{
    public PlayerUnitController(UnitModel unitData) : base(unitData)
    {
    }

    public void PlayMoveAnimation()
    {
        unitAnimationController.Idle();
    }
}
