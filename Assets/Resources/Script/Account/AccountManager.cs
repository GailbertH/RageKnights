using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    private static AccountManager instance = null;

    public static AccountManager Instance { get { return instance; } }

    private PlayerModel playerData;
    public PlayerModel PlayerData
    {
        get { return playerData; }
        set { playerData = value; }
    }

    void Awake()
    {
        instance = this;

        playerData = new PlayerModel
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
    }

    public void SaveData()
    { }

    public void LoadData()
    { }
}
