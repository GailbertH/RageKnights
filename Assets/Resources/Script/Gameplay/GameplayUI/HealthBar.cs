using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Transform health;
    [SerializeField] private Transform mana;

    private bool isMonsterHealth;
    private int maxHP;
    private int maxMP;

    public void Setup(StatusBarFields stats)
    {
        name.text = stats.name;
        maxHP = stats.maxHealthPoints;
        maxMP = stats.maxManaPoints;

        UpdateHealthPoints(stats.healthPoints);
        UpdateManaPoints(stats.manaPoints);
    }

    public void UpdateHealthPoints(int hp)
    {
        float healthPercent = (float)hp / (float)maxHP;
        if (healthPercent < 0 || healthPercent > 1)
        {
            healthPercent = healthPercent < 0 ? 0 : 1;
        }
        health.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void UpdateManaPoints(int mp)
    {
        float manaPercent = (float)mp / (float)maxMP;
        if (manaPercent < 0 || manaPercent > 1)
        {
            manaPercent = manaPercent < 0 ? 0 : 1;
        }
        mana.localScale = new Vector3(manaPercent, 1, 1);
    }
}
