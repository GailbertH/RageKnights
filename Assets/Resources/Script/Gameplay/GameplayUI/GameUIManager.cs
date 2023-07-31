﻿using UnityEngine;
using System.Collections;
using RageKnight;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Ink;
using System.Linq;

//TODO
//Separate GameControlsUI/GameAction in a different handler for easy scalability and editing.
public enum CombatUIMode
{
    NOT_IN_COMBAT,
    ACTION_SELECTION,
    TARGET_SELECTION,
    ACTION_EXECUTE
}

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Text coinsHeld;
    [SerializeField] private GameObject battleControlOverlay;
    [SerializeField] private PopupHandler popupHandler;
    [SerializeField] private MiddleUIHandler middleUIHandler;

    [SerializeField] private GameObject sloppy;

    [SerializeField] private Image characterInAction;
    [SerializeField] private Button attackButton;

    private CombatUIMode combatMode = CombatUIMode.NOT_IN_COMBAT;

    private const string ANIM_TIMING_NOTIF = "LB-AttackBtn-TimingNotif";
    private const string ANIM_TIMING_HIT = "LB-AttackBtn-TimingHit";

    private static GameUIManager instance = null;
    public static GameUIManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        mainCamera.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        MiddleUIHandler.Initialize();
    }

    public MiddleUIHandler MiddleUIHandler
    {
        get
        {
            return middleUIHandler;
        }
    }

    public HealthbarHandler HealthbarHandler
    {
        get
        {
            return middleUIHandler.HealthbarHandler;
        }
    }

    //-----Should be somewhere else
    private CombatAction buttonEvent;
    public CombatAction GetButtonEvent
    {
        get { return buttonEvent; }
    }


    public void ResetSelections()
    {
        buttonEvent = CombatAction.NONE;
    }

    public void AttackButton()
    {
        Debug.Log("Attack");
        buttonEvent = CombatAction.ATTACK;
        combatMode = CombatUIMode.TARGET_SELECTION;
    }
    //------

    #region Button response

    public void MenuButton()
    {
        OpenPopup(PopupList.MENU_UI);
    }

    public void DebugButton()
    {
        OpenPopup(PopupList.DATA_DEBUG_UI);
    }

    public void RetireAdventureButton()
    {
        Debug.Log("REtire Button");
    }

    public void ConfirmButton()
    {
        Debug.Log("Confirmation Button");
    }

    public void CancelButton()
    {
        Debug.Log("Cacnel Button");
    }

    #endregion

    public void OpenPopup(string popupName, bool fullHide = true)
    {
        popupHandler.OpenPopup(popupName);
    }

    public void OpenPopup(string popupName, object data, Action onClose)
    {
        popupHandler.OpenPopup(popupName, data, onClose);
    }

    //public void UpdateGold(long currentGold)
    //{
    //    coinsHeld.text = currentGold.ToString();
    //}

    public void DebugUpdateGamePlayState(string state)
    {
        coinsHeld.text = state.ToString();
    }

    public void UpdateControlMode(GameplayState currentState)
    {
        battleControlOverlay.SetActive(currentState != GameplayState.COMBAT);
        if (currentState != GameplayState.COMBAT)
            combatMode = CombatUIMode.NOT_IN_COMBAT;
    }

    public void UpdateMiddleUIModle(GameplayState currentState)
    {
        MiddleUIHandler.SetMiddleGround(currentState);
        if (splashArtsDict == null)
        {
            SetupSpriteCommandArt();
        }
    }
    public void ShowRageImage()
    {
        sloppy.GetComponent<Animation>().Play();
    }
    public void PreventPlayerCommands()
    {
        attackButton.interactable = false;
    }

    public void AllowPlayerCommands()
    {
        attackButton.interactable = true;
    }

    private Dictionary<string, Sprite> splashArtsDict = null;
    public void SetupSpriteCommandArt()
    {
        splashArtsDict = new Dictionary<string, Sprite>();
        var commands = GameManager.Instance.PlayerHandler.GetPlayerData.Select(x => x.commandField).ToList();
        foreach (var command in commands)
        {
            splashArtsDict.Add(command.unitCombatID, command.splashArt);
        }
    }
    public void UpdateChracterInAction(string combatId)
    {
        if (splashArtsDict.ContainsKey(combatId))
        {
            characterInAction.sprite = splashArtsDict[combatId];
        }   
        else 
        {
            characterInAction.sprite = null;
        }
        characterInAction.gameObject.SetActive(characterInAction.sprite != null);
    }
}
