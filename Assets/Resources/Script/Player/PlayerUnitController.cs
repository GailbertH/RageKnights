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
    [SerializeField] public UnitAnimatorController unitAnimationController;
    [SerializeField] private CombatPlacement combatPlacement;
    [SerializeField] private GameObject targetMarker = null;
    public CombatAction combatAction = CombatAction.NONE;
    private Vector3 initialPosition;

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
        initialPosition = transform.gameObject.transform.position;
        UnitData = unitData;
        if (UnitData.unitPrefab != null)
        {
            GameObject unitObject = Instantiate<GameObject>(UnitData.unitPrefab,transform) as GameObject;
            unitAnimationController = unitObject.GetComponent<UnitAnimatorController>();
            //spriteRenderer = unitObject.GetComponent<SpriteRenderer>();

            //spriteRenderer.sortingOrder = GetOrderInLayer;
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

    public virtual void ExecuteAction(Action onAnimationEnd = null)
    {
        if (combatAction == CombatAction.ATTACK)
        {
            Attack(onAnimationEnd);
        }
    }

    public virtual void Attack(Action onAnimationEnd = null)
    {
        unitAnimationController.Attack(onAnimationEnd);
    }
    public virtual void RunTowards(Transform targetLocation, float speed, Action callback = null)
    {
        float reachDistance = 1f;
        Vector3 frontPosition = targetLocation.transform.position + new Vector3(0, -0.25f, 0);
        if (Vector3.Distance(this.transform.position, frontPosition) <= reachDistance)
        {
            unitAnimationController.Idle();
            callback?.Invoke();
            return;
        }
        if (Vector3.Distance(targetLocation.transform.position, this.transform.position) <= 0.4f)
        {
            speed = 1f;
        }
        this.transform.position += (frontPosition - this.transform.position) * speed;
        unitAnimationController.Run();
    }

    public virtual void GoBackToInitialPosition(float speed, Action callback = null)
    {
        if (this.transform.position == initialPosition)
        {
            unitAnimationController.Idle();
            callback?.Invoke();
            return;
        }
        if (Vector3.Distance(initialPosition, this.transform.position) <= 0.4f)
        {
            speed = 1f;
        }
        this.transform.position += (initialPosition - this.transform.position) * speed;
        unitAnimationController.Run();
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
            Debug.Log("Hello");
            GameTargetingManager.Instance.TargetChange(GetUnitCombatId);
        }
    }
}

public class PlayerUnitController : UnitController
{
    public PlayerUnitController(UnitModel unitData) : base(unitData)
    {
    }
}
