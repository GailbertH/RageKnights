using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    private static AccountManager instance = null;

    public static AccountManager Instance { get { return instance; } }

    private AccountData accountData;
    public AccountData AccountData
    {
        get { return accountData; }
        set { accountData = value; }
    }

    void Awake()
    {
        instance = this;
        PlayerModel currentCharacterData = new PlayerModel
        {
            AttackPower = 2,
            DefensePower = 2,
            HealthPower = 2,
            RagePower = 2,
            RageIncrement = 0.5f,
            ActionGaugeIncrement = 1f,
            HealthPoints = 10f,
            ActionGaugePoints = 10f,
            RagePoints = 0,
            currentItemInUse = new ConsumableModel {
                id = "0000",
                quantity = 5
            },
            ItemCount = 5,
            WeapomStatBonus = 0,
            HealthStatBonus = 0,
            ArmorStatBonus = 0,
            BaseHealthPoints = 10f,
            MaxRagePoints = 100f,
            MaxActionGaugePoints = 100f,
            MaxItemCount = 10,
            AttackRageMultiplier = 2
        };

        ProgressTrackerModel currentProgress = new ProgressTrackerModel
        {
            CurrentRound = 0,
            CurrentStage = 1,
        };

        ProgressTrackerModel TotalProgress = new ProgressTrackerModel
        {
            CurrentRound = 0,
            CurrentStage = 1
        };

        AccountData = new AccountData
        {
            AccountId = "000042069",
            AccountName = "Kris Chiong",
            Gold = 100,
            CurrentCharacterData = currentCharacterData,
            CurrentProgress = currentProgress,
            ProgressTracker = TotalProgress
        };
    }

    public void UpdateStageProgress(int stage, int round)
    {
        ProgressTrackerModel currProgress = AccountData.CurrentProgress;
        currProgress.CurrentStage = stage;
        currProgress.CurrentRound = round;

        ProgressTrackerModel overallProgress = AccountData.ProgressTracker;
        if (overallProgress.CurrentStage <= stage)
        {
            overallProgress.CurrentStage = stage;
            overallProgress.CurrentRound = round > overallProgress.CurrentRound ? round : overallProgress.CurrentRound;
        }

        Debug.Log("Current " + currProgress.CurrentStage + " | " + currProgress.CurrentRound);
        Debug.Log("Overall " + overallProgress.CurrentStage + " | " + overallProgress.CurrentRound);
    }

    public void SaveData()
    { }

    public void LoadData()
    { }
}
public class AccountData
{
    public string AccountId;
    public string AccountName;
    public long Gold;
    public PlayerModel CurrentCharacterData; //Selected Character
    public PlayerModel[] CharacterData;
    public ProgressTrackerModel CurrentProgress;
    public ProgressTrackerModel ProgressTracker;
    //Inventory Data
}

public class ProgressTrackerModel
{
    public int CurrentRound;
    public int CurrentStage;
}


//Not account related
public class StageInfoModel
{
    public string Name;
    public int Round;
    public int Stage;
}


