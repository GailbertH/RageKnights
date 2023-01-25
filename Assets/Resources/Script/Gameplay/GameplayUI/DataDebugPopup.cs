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
    [SerializeField] private Text MaxHealthPoints;

    [SerializeField] private Text ManaPoints;
    [SerializeField] private Text MaxManaPoints;

    [SerializeField] private Text RagePoints;
    [SerializeField] private Text MaxRagePoints;

    [SerializeField] private Text RageInc;

    [SerializeField] private Text AttackPower;
    [SerializeField] private Text DefensePower;
    [SerializeField] private Text VitalityPower;
    #endregion
    //-----------------------------------------------------------------
    #region textboxinput
    [SerializeField] private Text HealthPointsNewValue;
    [SerializeField] private Text MaxHealthPointsNewValue;

    [SerializeField] private Text ManaPointsNewValue;
    [SerializeField] private Text MaxManaPointsNewValue;

    [SerializeField] private Text RagePointsNewValue;
    [SerializeField] private Text MaxRagePointsNewValue;

    [SerializeField] private Text RageIncNewValue;

    [SerializeField] private Text AttackPowerNewValue;
    [SerializeField] private Text DefensePowerNewValue;
    [SerializeField] private Text VitalityPowerNewValue;
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
        ManaPoints.text = "Mana Points - " + playerData.ManaPoints.ToString();
        RagePoints.text = "Rage Points - " + playerData.RagePoints.ToString();
        RageInc.text = "Rage Increment - " + playerData.RageIncrement.ToString();
        AttackPower.text = "Attack Power - " + playerData.AttackPower.ToString();
        DefensePower.text = "Defense Power - " + playerData.DefensePower.ToString();
        DefensePower.text = "Vitality Power - " + playerData.DefensePower.ToString();
    }

    public void SaveChanges()
    {
        int intValue = 0;

        if (int.TryParse(MaxHealthPointsNewValue.text, out intValue))
            playerData.MaxHealthPoints = intValue;
        if (int.TryParse(HealthPointsNewValue.text, out intValue))
            playerData.HealthPoints = playerData.MaxHealthPoints < intValue ? playerData.MaxHealthPoints : intValue;

        if (int.TryParse(MaxManaPointsNewValue.text, out intValue))
            playerData.MaxManaPoints = intValue;
        if (int.TryParse(ManaPointsNewValue.text, out intValue))
            playerData.ManaPoints = playerData.MaxManaPoints < intValue ? playerData.MaxManaPoints : intValue;

        if (int.TryParse(RageIncNewValue.text, out intValue))
            playerData.RageIncrement = intValue;


        if (int.TryParse(AttackPowerNewValue.text, out intValue))
            playerData.AttackPower = intValue;
        if (int.TryParse(DefensePowerNewValue.text, out intValue))
            playerData.DefensePower = intValue;
        if (int.TryParse(VitalityPowerNewValue.text, out intValue))
            playerData.VitalityPower = intValue;

        SetupData();
    }
}
