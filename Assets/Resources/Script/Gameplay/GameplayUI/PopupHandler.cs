using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupList
{
    public const string MENU_UI = "MenuUI";
    public const string RESULT_UI = "ResultUI";
    public const string DATA_DEBUG_UI = "DataDebuggerUI";
}


public class LobbyPopupList
{
    public const string STAGE_SELECT_UI = "StageSelectUI";
}


public class PopupHandler : MonoBehaviour
{
    [SerializeField] private GameObject OverlayFull;
    [SerializeField] private GameObject Overlay;
    [SerializeField] private List<GameObject> PopupList;
    [SerializeField] private bool isLobby;

    private List<string> currentOpenPopups = new List<string>();
    public void OpenPopup(string PopupName)
    {
        if (currentOpenPopups.Contains(PopupName))
            return;

        GameObject popup = PopupList.Find(x => x.name == PopupName);
        GameObject newPopup = Instantiate<GameObject>(popup, this.transform) as GameObject;
        Action popupCloseAction = delegate
        {
            OnPopupClose(PopupName);
        };

        newPopup.GetComponent<PopupBase>().Initialize(popupCloseAction);
        OnPopupOpen(PopupName);
        newPopup.SetActive(true);
    }


    public void OpenPopup(string PopupName, Action afterPopupCloseAction)
    {
        if (currentOpenPopups.Contains(PopupName))
            return;

        GameObject popup = PopupList.Find(x => x.name == PopupName);
        GameObject newPopup = Instantiate<GameObject>(popup, this.transform) as GameObject;
        Action popupCloseAction = delegate 
        {
            OnPopupClose(PopupName);
            afterPopupCloseAction.Invoke();
        };

        newPopup.GetComponent<PopupBase>().Initialize(popupCloseAction);
        OnPopupOpen(PopupName);
        newPopup.SetActive(true);
    }

    private void OnPopupOpen(string popupName)
    {
        currentOpenPopups.Add(popupName);
        GameManager.Instance?.PauseGame(true);
        OverlayFull.SetActive(true);
    }

    private void OnPopupClose(string popupName)
    {
        currentOpenPopups.Remove(popupName);
        GameManager.Instance?.PauseGame(false);
        OverlayFull.SetActive(false);
    }
}
