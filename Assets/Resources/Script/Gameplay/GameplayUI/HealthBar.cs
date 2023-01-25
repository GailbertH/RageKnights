using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform Health;
    [SerializeField] private Transform Mana;

    private bool isMonsterHealth;
    private int maxHP;
    private int maxMP;

    public void Setup(int hp, int maxHp, int mp, int maxMp)
    {
        maxHP = maxHp;
        maxMP = maxMp;
        UpdateHealthPoints(hp);
        UpdateManaPoints(mp);
    }

    public void UpdateHealthPoints(int hp)
    {
        float healthPercent = (float)hp / (float)maxHP;
        if (healthPercent < 0 || healthPercent > 1)
        {
            healthPercent = healthPercent < 0 ? 0 : 1;
        }
        Health.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void UpdateManaPoints(int mp)
    {
        float manaPercent = (float)mp / (float)maxMP;
        if (manaPercent < 0 || manaPercent > 1)
        {
            manaPercent = manaPercent < 0 ? 0 : 1;
        }
        Health.localScale = new Vector3(manaPercent, 1, 1);
    }
}
