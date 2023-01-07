using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataDebugPopup : PopupBase
{
    #region labels
    [SerializeField] private Text HealthPoints;
    [SerializeField] private Text BaseHealthPoints;
    [SerializeField] private Text RagePoints;
    [SerializeField] private Text MaxRagePoints;
    [SerializeField] private Text AGPoints;
    [SerializeField] private Text MaxAGPoints;
    [SerializeField] private Text RageInc;
    [SerializeField] private Text AGInc;
    [SerializeField] private Text AttackPower;
    [SerializeField] private Text DefensePower;
    [SerializeField] private Text RagePower;
    [SerializeField] private Text ItemCount;
    [SerializeField] private Text MaxItemCount;
    [SerializeField] private Text AttackRageMultiplier;
    #endregion
    //-----------------------------------------------------------------
    #region textboxinput
    [SerializeField] private Text HealthPointsNewValue;
    [SerializeField] private Text BaseHealthPointsNewValue;
    [SerializeField] private Text RagePointsNewValue;
    [SerializeField] private Text MaxRagePointsNewValue;
    [SerializeField] private Text AGPointsNewValue;
    [SerializeField] private Text MaxAGPointsNewValue;
    [SerializeField] private Text RageIncNewValue;
    [SerializeField] private Text AGIncNewValue;
    [SerializeField] private Text AttackPowerNewValue;
    [SerializeField] private Text DefensePowerNewValue;
    [SerializeField] private Text RagePowerNewValue;
    [SerializeField] private Text ItemCountNewValue;
    [SerializeField] private Text MaxItemCountNewValue;
    [SerializeField] private Text AttackRageMultiplierNewValue;
    #endregion

    private PlayerUnitModel playerData = null;

    public override void Initialize(Action OnCloseAction, object data = null)
    {
        base.Initialize(OnCloseAction);
        playerData = GameManager.Instance?.PlayerHandler?.GetPlayerData;
        if (playerData != null)
        {
            SetupData();
        }
    }

    public void SetupData()
    {
        HealthPoints.text = "Health Points - " + playerData.HealthPoints.ToString();
        RagePoints.text = "Rage Points - " + playerData.RagePoints.ToString();
        AGPoints.text = "Action Points - " + playerData.ActionGaugePoints.ToString();
        RageInc.text = "Rage Increment - " + playerData.RageIncrement.ToString();
        AGInc.text = "Action Increment - " + playerData.ActionGaugeIncrement.ToString();
        AttackPower.text = "Attack Power - " + playerData.AttackPower.ToString();
        DefensePower.text = "Defense Power - " + playerData.DefensePower.ToString();
    }

    public void SaveChanges()
    {
        float floatValue = 0;

        if (float.TryParse(BaseHealthPointsNewValue.text, out floatValue))
            playerData.MaxHealthPoints = floatValue;
        if (float.TryParse(HealthPointsNewValue.text, out floatValue))
            playerData.HealthPoints = playerData.MaxHealthPoints < floatValue ? playerData.MaxHealthPoints : floatValue;

        if (float.TryParse(RageIncNewValue.text, out floatValue))
            playerData.RageIncrement = floatValue;
        if (float.TryParse(AGIncNewValue.text, out floatValue))
            playerData.ActionGaugeIncrement = floatValue;


        int intValue = 0;
        if (int.TryParse(AttackPowerNewValue.text, out intValue))
            playerData.AttackPower = intValue;
        if (int.TryParse(DefensePowerNewValue.text, out intValue))
            playerData.DefensePower = intValue;

        SetupData();
    }
}
