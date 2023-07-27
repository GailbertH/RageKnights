using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerUnitstatusBarCopy;
    [SerializeField] private GameObject enemyUnitstatusBarCopy;
    [SerializeField] private Transform playerSideParent;
    [SerializeField] private Transform enemySideParent;
    private List<HealthBar> playerUnitHealthBar = new List<HealthBar>();
    private List<HealthBar> enemyUnitHealthBar = new List<HealthBar>();

    public void SetUIActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
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
            playerUnitHealthBar.Add(phealthBar);
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
            enemyUnitHealthBar.Add(ehealthBar);
        }
    }
}
