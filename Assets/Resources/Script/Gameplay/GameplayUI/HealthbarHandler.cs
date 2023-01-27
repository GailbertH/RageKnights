using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    [SerializeField] private GameObject statusBarCopy;
    [SerializeField] private Transform playerSideParent;
    [SerializeField] private Transform enemySideParent;

    public void SetUIActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    public void SetupPlayerSide(List<StatusBarFields> playerStatus)
    {
        for (int i = 0; i < playerStatus.Count; i++)
        {
            GameObject playerObject = Instantiate<GameObject>(statusBarCopy,
                playerSideParent) as GameObject;
            playerObject.GetComponent<HealthBar>().Setup(playerStatus[i]);
            playerObject.SetActive(true);
        }
    }

    public void SetupEnemySide(int unitCount)
    {
        for (int i = 0; i < unitCount; i++)
        {
            GameObject enemyObject = Instantiate<GameObject>(statusBarCopy,
                enemySideParent) as GameObject;
            //enemyObject.GetComponent<HealthBar>().Setup(playerStatus[i]);
            enemyObject.SetActive(true);
        }
    }

    public void ShowHealthBar()
    {

    }

    public void HideHealthBar()
    {

    }

}
