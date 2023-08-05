using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerUnitstatusBarCopy;
    [SerializeField] private GameObject enemyUnitstatusBarCopy;
    [SerializeField] private Transform playerSideParent;
    [SerializeField] private Transform enemySideParent;
    private List<HealthBar> healthBars = new List<HealthBar>();

    public void SetUIActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    private HealthBar GetTargetHealthBar(string combatId)
    {
        return healthBars.Find(x => x.GetUnitCombatID == combatId);
    }
    public void UpdateActiveStatus(string combatId)
    {
        foreach (HealthBar healthBar in healthBars)
        {
            healthBar.UpdateActiveStatus(combatId);
        }
    }

    public void UpdateTagetStatus(string combatId)
    {
        foreach (HealthBar healthBar in healthBars)
        {
            healthBar.UpdateTargetState(combatId);
        }
    }

    public void UpdateHealthPoints(string combatId, int curHP)
    {
        var targetHealthBar = GetTargetHealthBar(combatId);
        if (targetHealthBar != null)
        { 
            targetHealthBar.UpdateHealthPoints(curHP);
        }
    }

    public void SetupPlayerSide(List<StatusBarFields> playerStatus)
    {
        for (int i = 0; i < playerStatus.Count; i++)
        {
            GameObject playerObject = Instantiate<GameObject>(playerUnitstatusBarCopy,
                playerSideParent) as GameObject;
            var phealthBar = playerObject.GetComponent<HealthBar>();
            phealthBar.Setup(playerStatus[i]);
            phealthBar.gameObject.SetActive(true);
            healthBars.Add(phealthBar);
        }
    }

    public void SetupEnemySide(List<StatusBarFields> enemyStatus)
    {
        for (int i = 0; i < enemyStatus.Count; i++)
        {
            GameObject enemyObject = Instantiate<GameObject>(enemyUnitstatusBarCopy,
                enemySideParent) as GameObject;
            var ehealthBar = enemyObject.GetComponent<HealthBar>();
            enemyStatus[i].isEnemy = true;
            ehealthBar.Setup(enemyStatus[i]);
            ehealthBar.gameObject.SetActive(true);
            healthBars.Add(ehealthBar);
        }
    }
}
