using UnityEngine;
using System.Collections;
using RageKnight;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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
    //------  Game Action Button --------
    [SerializeField] private Button attackButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button rageButton;

    [SerializeField] private Image itemButtonIcon;
    [SerializeField] private Text itemButtonText;

    [SerializeField] private Transform playerRageMeter;

    [SerializeField] private Animation animAttackButton;
    private CombatUIMode combatMode = CombatUIMode.NOT_IN_COMBAT;

    private const string ANIM_TIMING_NOTIF = "LB-AttackBtn-TimingNotif";
    private const string ANIM_TIMING_HIT = "LB-AttackBtn-TimingHit";

    private static GameUIManager instance = null;
    public static GameUIManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        mainCamera.gameObject.SetActive(false);
        MiddleUIHandler.Initialize();
    }

    private bool isDragging = false;
    public bool OnJoyStickDrag
    {
        set { isDragging = value; }
        get { return isDragging; }
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

    public ProgressBarHandler ProgressbarHandler
    {
        get
        {
            return middleUIHandler.ProgressbarHandler;
        }
    }

    //-----Should be somewhere else
    private CombatAction buttonEvent;
    public CombatAction GetButtonEvent
    {
        get { return buttonEvent; }
    }

    private List<string> targets = new List<string>();
    public List<string> GetTargets
    {
        get { return targets; }
    }

    private bool isActionSelectionDone = false;
    public bool GetIsActionSelectionDone
    {
        get { return isActionSelectionDone; }
    }

    public void ResetSelections()
    {
        isActionSelectionDone = false;
        buttonEvent = CombatAction.NONE;
        targets = new List<string>();
    }

    public void AttackButton()
    {
        buttonEvent = CombatAction.ATTACK;
        combatMode = CombatUIMode.TARGET_SELECTION;
    }

    public void PreventPlayerCommands()
    {
        attackButton.interactable = false;
    }

    public void AllowPlayerCommands()
    {
        attackButton.interactable = true;
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
    }

    public void ItemButtonStatus(bool isActive)
    {
        itemButton.interactable = isActive;
    }

    public void UpdateRageMeter(float currentValue, float maxValue)
    {
        float actionGaugePercent = currentValue / maxValue;
        if (actionGaugePercent < 0 || actionGaugePercent > 1)
        {
            actionGaugePercent = actionGaugePercent < 0 ? 0 : 1;
        }
        playerRageMeter.localScale = new Vector3(actionGaugePercent, 1, 1);
    }

    public void ShowRageImage()
    {
        sloppy.GetComponent<Animation>().Play();
    }

    public void AddTarget(string unitCombatID)
    {
        if (targets.Contains(unitCombatID))
            return;

        targets.Add(unitCombatID);
    }

    public void TargetSelectionDone()
    {
        if (targets.Count <= 0)
            return;

        isActionSelectionDone = true;
    }
}
