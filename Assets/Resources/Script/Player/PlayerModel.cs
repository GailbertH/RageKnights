using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public float HealthPoints { get; set; }
    public float RagePoints { get; set; }
    public float ActionGaugePoints { get; set; }

    public float RageIncrement { get; set; }
    public float ActionGaugeIncrement { get; set; }

    public int AttackPower { get; set; }
    public int DefensePower { get; set; }
    public int HealthPower { get; set; }
    public int RagePower { get; set; }

    public int ItemCount { get; set; }

    public int WeapomStatBonus { get; set; } //ATTACK
    public int HealthStatBonus { get; set; } //DEFENSE
    public int ArmorStatBonus { get; set; } //HEALTH

    //Must be moved somewhere
    public float BaseHealthPoints { get; set; }
    public float MaxRagePoints { get; set; }
    public float MaxActionGaugePoints { get; set; }
}
