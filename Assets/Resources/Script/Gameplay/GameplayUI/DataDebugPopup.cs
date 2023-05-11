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
        playerData = GameManager.Instance?.PlayerHandler?.GetPlayerData[0];
        if (playerData != null)
        {
            SetupData();
        }
    }

    public void SetupData()
    {
        HealthPoints.text = "Health Points - " + playerData.healthPoints.ToString();
        ManaPoints.text = "Mana Points - " + playerData.manaPoints.ToString();
        RagePoints.text = "Rage Points - " + playerData.ragePoints.ToString();
        RageInc.text = "Rage Increment - " + playerData.rageIncrement.ToString();
        AttackPower.text = "Attack Power - " + playerData.attackPower.ToString();
        DefensePower.text = "Defense Power - " + playerData.defensePower.ToString();
        DefensePower.text = "Vitality Power - " + playerData.defensePower.ToString();
    }

    public void SaveChanges()
    {
        int intValue = 0;

        if (int.TryParse(MaxHealthPointsNewValue.text, out intValue))
            playerData.maxHealthPoints = intValue;
        if (int.TryParse(HealthPointsNewValue.text, out intValue))
            playerData.healthPoints = playerData.maxHealthPoints < intValue ? playerData.maxHealthPoints : intValue;

        if (int.TryParse(MaxManaPointsNewValue.text, out intValue))
            playerData.maxManaPoints = intValue;
        if (int.TryParse(ManaPointsNewValue.text, out intValue))
            playerData.manaPoints = playerData.maxManaPoints < intValue ? playerData.maxManaPoints : intValue;

        if (int.TryParse(RageIncNewValue.text, out intValue))
            playerData.rageIncrement = intValue;


        if (int.TryParse(AttackPowerNewValue.text, out intValue))
            playerData.attackPower = intValue;
        if (int.TryParse(DefensePowerNewValue.text, out intValue))
            playerData.defensePower = intValue;
        if (int.TryParse(VitalityPowerNewValue.text, out intValue))
            playerData.vitalityPower = intValue;

        SetupData();
    }
}
