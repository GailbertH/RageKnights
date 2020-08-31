using UnityEngine;
using System.Collections;
using RageKnight;
using UnityEngine.UI;
using System;

public enum BasicMovements {
    None = 0,
    Idle = 1,
    Attack = 2,
    Skill = 3,
}

public enum SkillMovements {
    None = 0,
    Rage = 1,
    Heal = 2,
}

//TODO
//Separate GameControlsUI/GameAction in a different handler for easy scalability and editing.


public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Text coinsHeld;
    [SerializeField] private GameObject BattleControls;
    [SerializeField] private GameObject AdventureControls;
    [SerializeField] private PopupHandler popupHandler;
    [SerializeField] private MiddleUIHandler middleUIHandler;

    //------  Game Action Button --------
    [SerializeField] private Button attackButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button rageButton;
    [SerializeField] private Button bossButton;

    [SerializeField] private Image itemButtonIcon;
    [SerializeField] private Text itemButtonText;

    [SerializeField] private Transform playerRageMeter;

    [SerializeField] private Animation animAttackButton;
    private const string ANIM_TIMING_NOTIF = "LB-AttackBtn-TimingNotif";
    private const string ANIM_TIMING_HIT = "LB-AttackBtn-TimingHit";

    private Coroutine initialize;

    void Awake()
    {
        mainCamera.gameObject.SetActive(false);
        initialize = StartCoroutine(Initializer());
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

    public BasicMovements buttonEvents;

    public void AttackButton()
    {
        buttonEvents = BasicMovements.Attack;
    }

    public SkillMovements skillEvents;
    public void SkillButton()
    {
        skillEvents = SkillMovements.Rage;
    }

    public void HealButtton()
    {
        skillEvents = SkillMovements.Heal;
    }

    public void BossButton()
    {
        Debug.Log("Boss Button Click");
        GameManager.Instance.IncrementStage(true);
    }
    public void BossButtonActive(bool activate)
    {
        bossButton.gameObject.SetActive(activate);
    }

    public bool IsTimingPlaying()
    {
        return animAttackButton.IsPlaying(ANIM_TIMING_NOTIF);
    }
    public void PlayTimingNotif()
    {
        animAttackButton.Play(ANIM_TIMING_NOTIF);
    }
    public void PlayTimingHit()
    {
        animAttackButton.Play(ANIM_TIMING_HIT);
    }

    #region Button response

    public void MenuButton()
    {
        OpenPopup(PopupList.MENU_UI);
    }

    public void StoreButton()
    {
        OpenPopup(PopupList.STORE_UI, false);
    }

    public void PreparationButton()
    {
        OpenPopup(PopupList.UPGRADE_UI, false);
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

    public void OpenPopup(string popupName, Action onClose)
    {
        popupHandler.OpenPopup(popupName, onClose);
    }

    private IEnumerator Initializer()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.GameUIManager = this;
        MiddleUIHandler.Initialize();
        BossButtonActive(false);
    }

    public void UpdateGold(long currentGold)
    {
        coinsHeld.text = currentGold.ToString();
    }

    public void UpdateControlMode(GameplayState currentState)
    {
        BattleControls.SetActive(currentState == GameplayState.COMBAT);
        AdventureControls.SetActive(currentState == GameplayState.ADVENTURE);
    }

    public void UpdateMiddleUIModle(GameplayState currentState)
    {
        MiddleUIHandler.SetMiddleGround(currentState);
    }

    public void UpdateItemButtonText(int itemCount)
    {
        itemButtonText.text = "x " + itemCount;
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
}
