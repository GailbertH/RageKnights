using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : PopupBase
{
    [SerializeField] private Text killCount;
    [SerializeField] private Text goldEarnings;

    public override void Initialize(Action OnCloseAction, object data = null)
    {
        base.Initialize(OnCloseAction, data);
        CombatTracker combatTracker = (CombatTracker)data;
        killCount.text = combatTracker.killCount.ToString();
        goldEarnings.text = combatTracker.goldEarned.ToString();
    }

    //Give reward
    public void ExitButton()
    {
        CloseButton();
    }
}
