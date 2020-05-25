using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    //Turn into a list someday
    [SerializeField] private GameObject HomeContent;
    [SerializeField] private GameObject UpgradeContent;
    [SerializeField] private GameObject TeamContent;
    [SerializeField] private GameObject ShopContent;
    [SerializeField] private GameObject SettingContent; //Not official just place holder
    [SerializeField] private LobbyOptionHandler OptionHandler;

    [SerializeField] GameObject genericPopup;
    private GameObject currentContentShowing = null;

    private static LobbyManager instance;
    public static LobbyManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        OptionHandler.Initialize();
    }

    public void PlayButtonClick()
	{
        if(LoadingManager.Instance != null)
        {
            LoadingManager.Instance.SetSceneToUnload (SceneNames.LOBBY_SCENE);
            LoadingManager.Instance.SetSceneToLoad (SceneNames.GAME_UI + "," + SceneNames.GAME_SCENE);
            LoadingManager.Instance.LoadGameScene ();
        }
        else
        {
            Debug.LogError("Loading Manager is missing");
        }
	}

    private void HideCurrentContent()
    {
        if (currentContentShowing != null)
        {
            currentContentShowing.SetActive(false);
        }
    }

    public void ShowHomeContent()
    {
        HideCurrentContent();
        currentContentShowing = HomeContent;
        currentContentShowing.SetActive(true);
    }

    public void ShowUpgradeContent()
    {
        HideCurrentContent();
        currentContentShowing = UpgradeContent;
        currentContentShowing.SetActive(true);
    }

    public void ShowTeamContent()
    {
        HideCurrentContent();
        currentContentShowing = TeamContent;
        currentContentShowing.SetActive(true);
    }

    public void ShowShopContent()
    {
        HideCurrentContent();
        currentContentShowing = ShopContent;
        currentContentShowing.SetActive(true);
    }

    public void ShowSettingContent()
    {
        HideCurrentContent();
        currentContentShowing = SettingContent;
        currentContentShowing.SetActive(true);
    }
}
