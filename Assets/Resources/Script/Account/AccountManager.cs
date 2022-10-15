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
        PlayerUnitModel currentCharacterData = new PlayerUnitModel
        {
            AttackPower = 2,
            DefensePower = 2,
            HealthPower = 2,
            RageIncrement = 0.5f,
            ActionGaugeIncrement = 0.2f,
            HealthPoints = 30f,
            ActionGaugePoints = 10f,
            RagePoints = 0,
        };
        string jsonAccountData = JsonUtility.ToJson(currentCharacterData);
        Debug.Log("FAKE DATA : " + jsonAccountData);

        ProgressTrackerModel currentProgress = new ProgressTrackerModel
        {
            CurrentRound = 5,
            CurrentStage = 1,
        };

        ProgressTrackerModel TotalProgress = new ProgressTrackerModel
        {
            CurrentRound = 5,
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

    public void UpdateStageProgress(int stage, int round, bool hasActiveGame)
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
        AccountData.HasCurrentActiveGame = hasActiveGame;
        SaveData();
    }

    public long AddGold(long goldToAdd, bool noSave = false)
    {
        accountData.Gold += goldToAdd;
        if(noSave == false)
            SaveData();
        return accountData.Gold;
    }

    //TODO at the end of each stage it saves twice, and we don't want that. D:
    private void SaveData()
    {
        string jsonAccountData = JsonUtility.ToJson(AccountData);
        Debug.Log("SAVE DATA : " + jsonAccountData);
        PlayerPrefs.SetString("TempAccountData", jsonAccountData);
    }

    private void LoadData()
    {
        string jsonAccountData = PlayerPrefs.GetString("TempAccountData");
        Debug.Log("LOAD DATA : " + jsonAccountData);
        try
        {
            GenerateAccount();
            //AccountData = JsonUtility.FromJson<AccountData>(jsonAccountData);
            SaveData();
        }
        catch
        {
            Debug.LogError("Data corrupted, creating new data");
            GenerateAccount(); 
        }
    }

    public void SetActiveGame(bool hasActiveGame, bool noSave = false)
    {
        AccountData.HasCurrentActiveGame = hasActiveGame;
        if (noSave == true)
            SaveData();
    }

    public void ResetPlayerStatus()
    {
        AccountData.CurrentCharacterData.HealthPoints = AccountData.CurrentCharacterData.MaxHealthPoints;
        AccountData.CurrentCharacterData.ActionGaugePoints = 0;
        AccountData.CurrentCharacterData.RagePoints = 0;
    }
}

[Serializable]
public class AccountData
{
    public string AccountId;
    public string AccountName;
    public long Gold;
    public bool HasCurrentActiveGame;
    public PlayerUnitModel CurrentCharacterData; //Selected Character
    public PlayerUnitModel[] CharacterData;
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


