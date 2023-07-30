using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Transform health;
    [SerializeField] private Transform mana;
    [SerializeField] private GameObject activateHiglight;

    private string unitCombatID;
    private bool isEnemyHealth;
    private int maxHP;
    private int maxMP;

    public string GetUnitCombatID
    {
        get { return unitCombatID; }
    }

    public bool GetIsEnemy
    {
        get { return isEnemyHealth;  }
    }

    public void Setup(StatusBarFields stats)
    {
        unitCombatID = stats.unitCombatID;
        icon.sprite = stats.icon;
        maxHP = stats.maxHealthPoints;
        maxMP = stats.maxManaPoints;
        isEnemyHealth = stats.isEnemy;

        UpdateHealthPoints(stats.healthPoints);
        UpdateManaPoints(stats.manaPoints);
    }

    public void UpdateActiveStatus(string activeUnitCombatIt)
    {
        activateHiglight.SetActive(unitCombatID == activeUnitCombatIt);
    }

    public void UpdateTargetState(string activeUnitCombatIt)
    {
        Debug.Log("Current Target " + activeUnitCombatIt);
    }

    public void UpdateHealthPoints(int curHP)
    {
        float healthPercent = (float)curHP / (float)maxHP;
        if (healthPercent < 0 || healthPercent > 1)
        {
            healthPercent = healthPercent < 0 ? 0 : 1;
        }
        health.localScale = new Vector3(healthPercent, 1, 1);
    }

    public void UpdateManaPoints(int mp)
    {
        if (mana == null)
            return;

        float manaPercent = (float)mp / (float)maxMP;
        if (manaPercent < 0 || manaPercent > 1)
        {
            manaPercent = manaPercent < 0 ? 0 : 1;
        }
        mana.localScale = new Vector3(manaPercent, 1, 1);
    }

    public void buttonSelect()
    {
        GameTargetingManager.Instance.AddTarget(unitCombatID);
    }
}
