using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum UnitSide
{   
    PLAYER, 
    ENEMY 
}
public class GameTargetingManager : MonoBehaviour
{
    public delegate void UnitTargetChange(string targetID);
    private event UnitTargetChange OnUnitTargetChangeEvent;

    private bool isDamagingType;     //isDamageType should be changed to enum someday
    private int targetCount;

    private static GameTargetingManager instance = null;
    public static GameTargetingManager Instance { get { return instance; } }
    private Dictionary<string, string> prevTarget = new Dictionary<string, string>();
    private List<string> targets = new List<string>();
    private List<string> playerIds = new List<string>();
    private List<string> enemyIds = new List<string>();
    public List<string> GetTargets
    {
        get { return targets; }
    }

    private bool isTargetSelectionDone = false;
    public bool GetIsTargetSelectionDone
    {
        get { return GetTargets.Count > 0; }
    }

    private void Awake()
    {
        instance = this;
    }
    public void InitializeTargetCondition(bool isDamageType, int numberOfTargets = 1)
    {
        isDamagingType = isDamageType;
        targetCount = numberOfTargets;
    }

    public void AddTargetUnitToTheList(string combatId, UnitSide unitSide)
    {
        if (unitSide == UnitSide.PLAYER)
        {
            playerIds.Add(combatId);
        }
        else
        {
            enemyIds.Add(combatId);
        }
    }

    public void RemoveTargetUnitToTheList(string combatId, UnitSide unitSide)
    {
        if (unitSide == UnitSide.PLAYER)
        {
            playerIds.Remove(combatId);
        }
        else
        {
            enemyIds.Remove(combatId);
        }
    }

    public void OnUnitTargetChange(UnitTargetChange method)
    {
        //Debug.Log("Added new methods");
        OnUnitTargetChangeEvent += method;
    }

    public void RemoveOnUnitTargetChange(UnitTargetChange method)
    {
        //Debug.Log("Delete new methods");
        OnUnitTargetChangeEvent -= method;
    }

    public void TargetChange(string unitCombatID)
    {
        OnUnitTargetChangeEvent.Invoke(unitCombatID);
        targets.RemoveAll(t => t != unitCombatID);
        targets.Add(unitCombatID);
        GameUIManager.Instance.HealthbarHandler.UpdateTagetStatus(unitCombatID);
    }

    public void AddTarget(string unitCombatID)
    {
        //Debug.Log("Target select " +unitCombatID);
        TargetChange(unitCombatID);
    }

    public void ResetSelections()
    {
        isTargetSelectionDone = false;
        OnUnitTargetChangeEvent.Invoke(string.Empty);
        targets = new List<string>();
        GameUIManager.Instance.HealthbarHandler.UpdateTagetStatus("");
    }
    public void SetRandomPlayerUnitTarget()
    {
        System.Random rnd = new System.Random();
        int r = rnd.Next(playerIds.Count);
        AddTarget(playerIds[r]);
    }
}
