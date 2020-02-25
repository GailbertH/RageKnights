﻿using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupList
{
    public const string MENU_UI = "MenuUI";
    public const string UPGRADE_UI = "UpgradeUI";
    public const string STORE_UI = "StoreUI";
    public const string RESULT_UI = "ResultUI";
}

public class PopupHandler : MonoBehaviour
{
    [SerializeField] private GameObject Overlay;
    [SerializeField] private List<GameObject> PopupList;
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
        GameManager.Instance.PauseGame(true);
        Overlay.SetActive(true);
    }

    private void OnPopupClose(string popupName)
    {
        currentOpenPopups.Remove(popupName);
        GameManager.Instance.PauseGame(false);
        Overlay.SetActive(false);
    }
}
