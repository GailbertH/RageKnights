using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyOptionHandler : MonoBehaviour
{
    [SerializeField] private Button HomeButton;
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private Button TeamButton;
    [SerializeField] private Button ShopButton;
    [SerializeField] private Button SettingButton;
    private Button currentActiveButton;

    public void Initialize()
    {
        OnPressHomeButton();
    }

    public void OnPressHomeButton()
    {
        HomeButton.interactable = false;
        ReactivateCurrentActiveButton();
        currentActiveButton = HomeButton;
        LobbyManager.Instance?.ShowHomeContent();
    }

    public void OnPressUpgradeButtonButton()
    {
        UpgradeButton.interactable = false;
        ReactivateCurrentActiveButton();
        currentActiveButton = UpgradeButton;
        LobbyManager.Instance?.ShowUpgradeContent();
    }

    public void OnPressTeamButton()
    {
        TeamButton.interactable = false;
        ReactivateCurrentActiveButton();
        currentActiveButton = TeamButton;
        LobbyManager.Instance?.ShowTeamContent();
    }

    public void OnPressShopButton()
    {
        ShopButton.interactable = false;
        ReactivateCurrentActiveButton();
        currentActiveButton = ShopButton;
        LobbyManager.Instance?.ShowShopContent();
    }

    public void OnPressSettingButton()
    {
        SettingButton.interactable = false;
        ReactivateCurrentActiveButton();
        currentActiveButton = SettingButton;
        LobbyManager.Instance?.ShowSettingContent();
    }

    private void ReactivateCurrentActiveButton()
    {
        if (currentActiveButton != null)
        {
            currentActiveButton.interactable = true;
        }
    }
}
