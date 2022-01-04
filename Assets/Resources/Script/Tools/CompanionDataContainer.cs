using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompanionModel
{
    public int Id;
    public string Name;
    public string role; //don't use this yet
    public string job; // dont use this yet
    public float HealthPoints;
    public int AttackPower;
    public int DefensePower;
    public int AttackCoolDownLength;
}

public class CompanionDataContainer : MonoBehaviour
{
    [SerializeField] private List<CompanionModel> CompanionDataList; 
}
