using System;
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
        if (PlayerPrefs.HasKey("TempAccountData"))
        {
            LoadData();
        }
        else
        {
            GenerateAccount();
        }
    }

    public void GenerateAccount()
    {
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
            currentItemInUse = new ConsumableModel
            {
                id = "0000",
                quantity = 5
            },
            WeapomStatBonus = 0,
            HealthStatBonus = 0,
            ArmorStatBonus = 0,
            BaseHealthPoints = 10f,
            MaxRagePoints = 100f,
            MaxActionGaugePoints = 100f,
            AttackRageMultiplier = 2
        };
        string jsonAccountData = JsonUtility.ToJson(currentCharacterData);
        Debug.Log("FAKE DATA : " + jsonAccountData);

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

        SaveData();
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

        accountData.CurrentProgress = currProgress;
        accountData.ProgressTracker = overallProgress;
        SaveData();
    }

    public long AddGold(long goldToAdd)
    {
        accountData.Gold += goldToAdd;
        SaveData();
        return accountData.Gold;
    }

    //TODO at the end of the stage it saves twice, and we don't want that. D:
    public void SaveData()
    {
        string jsonAccountData = JsonUtility.ToJson(AccountData);
        Debug.Log("SAVE DATA : " + jsonAccountData);
        PlayerPrefs.SetString("TempAccountData", jsonAccountData);
    }

    public void LoadData()
    {
        string jsonAccountData = PlayerPrefs.GetString("TempAccountData");
        Debug.Log("LOAD DATA : " + jsonAccountData);
        try
        {
            AccountData = JsonUtility.FromJson<AccountData>(jsonAccountData);
            SaveData();
        }
        catch
        {
            Debug.LogError("Data corrupted, creating new data");
            GenerateAccount(); 
        }
    }
}

[Serializable]
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

[Serializable]
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


