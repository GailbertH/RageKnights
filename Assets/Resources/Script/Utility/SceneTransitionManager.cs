using RageKnight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionKey
{
    SPLASH_TO_LOBBY,
    LOBBY_TO_ADVENTURE,
    ADVENTURE_TO_COMBAT,
    COMBAT_TO_ADVENTURE,
    ADVENTURE_TO_LOBBY,
    COMBAT_TO_LOBBY,
}
public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance { get { return instance; } }
    private Coroutine transitionRoutine;

    Dictionary<string, AsyncOperation> sceneAsyncOperations;
    private void Awake()
    {
        instance = this;
        sceneAsyncOperations = new Dictionary<string, AsyncOperation>();
    }

    public void StartTransition(TransitionKey transitionKey, Action callback = null)
    {
        if (transitionRoutine != null)
        {
            Debug.LogWarning("Transition in process");
        }

        switch (transitionKey)
        {
            case TransitionKey.SPLASH_TO_LOBBY:
                transitionRoutine = StartCoroutine(SplashToLobby(callback));
                break;

            case TransitionKey.LOBBY_TO_ADVENTURE:
                transitionRoutine = StartCoroutine(LobbyToAdventure(callback));
                break;

            case TransitionKey.ADVENTURE_TO_COMBAT:
                transitionRoutine = StartCoroutine(AdventureToCombat(callback));
                break;

            case TransitionKey.COMBAT_TO_ADVENTURE:
                transitionRoutine = StartCoroutine(CombatToAdventure(callback));
                break;

            default:
                Debug.LogError("Invalid Transition Key.");
                break;
        }
    }

    private AsyncOperation GetSceneAsyncOperation(string sceneName)
    {
        if (sceneAsyncOperations.ContainsKey(sceneName))
        {
            return sceneAsyncOperations[sceneName];
        }
        return null;
    }


    private IEnumerator SplashToLobby(Action callback)
    {
        Debug.Log("TTTT - SplashToLobby");
        LoadScene(SceneNames.LOBBY_SCENE);
        yield return new WaitForEndOfFrame();
        AsyncOperation asyncOp = GetSceneAsyncOperation(SceneNames.LOBBY_SCENE);

        LoadScene(SceneNames.DATA_SCENE);
        yield return new WaitForEndOfFrame();
        while (asyncOp == null || asyncOp.isDone == false)
        {
            Debug.Log(asyncOp.progress);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.LOBBY_SCENE));


        callback();
    }
    private IEnumerator LobbyToAdventure(Action callback)
    {
        Debug.Log("TTTT - LobbyToAdventure");
        LoadingManager.Instance.ShowLoading();
        yield return new WaitForEndOfFrame();

        UnloadScene(SceneNames.LOBBY_SCENE);
        yield return new WaitForEndOfFrame();

        //determine stage to load
        //load assest
        //load music
        LoadScene(SceneNames.ADVENTURE_SCENE);
        AsyncOperation asyncOp = GetSceneAsyncOperation(SceneNames.ADVENTURE_SCENE);
        yield return new WaitForEndOfFrame();
        while (asyncOp == null || asyncOp.isDone == false)
        {
            Debug.Log(asyncOp.progress);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.ADVENTURE_SCENE));

        LoadScene(SceneNames.ADVENTURE_UI);
        yield return new WaitForEndOfFrame();
        while (asyncOp == null || asyncOp.isDone == false)
        {
            Debug.Log(asyncOp.progress);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        LoadingManager.Instance.OnLoadBarFull();
    }

    private IEnumerator AdventureToCombat(Action callback)
    {
        Debug.Log("TTTT - LobbyToAdventure");
        LoadingManager.Instance.ShowLoading();
        yield return new WaitForEndOfFrame();

        UnloadScene(SceneNames.ADVENTURE_SCENE);
        UnloadScene(SceneNames.ADVENTURE_UI);
        yield return new WaitForEndOfFrame();

        //determine stage to load
        //load assest
        //load music
        LoadScene(SceneNames.GAME_SCENE);
        AsyncOperation asyncOp = GetSceneAsyncOperation(SceneNames.GAME_SCENE);
        yield return new WaitForEndOfFrame();
        while (asyncOp == null || asyncOp.isDone == false)
        {
            Debug.Log(asyncOp.progress);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.GAME_SCENE));

        LoadScene(SceneNames.COMBAT_UI);
        yield return new WaitForEndOfFrame();
        while (asyncOp == null || asyncOp.isDone == false)
        {
            Debug.Log(asyncOp.progress);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        LoadingManager.Instance.OnLoadBarFull();
        GameManager.Instance.Initialized();
    }

    private IEnumerator CombatToAdventure(Action callback)
    {
        Debug.Log("TTTT - LobbyToAdventure");
        LoadingManager.Instance.ShowLoading();
        yield return new WaitForEndOfFrame();

        UnloadScene(SceneNames.GAME_SCENE);
        UnloadScene(SceneNames.COMBAT_UI);
        yield return new WaitForEndOfFrame();

        //determine stage to load
        //load assest
        //load music
        LoadScene(SceneNames.ADVENTURE_SCENE);
        AsyncOperation asyncOp = GetSceneAsyncOperation(SceneNames.ADVENTURE_SCENE);
        yield return new WaitForEndOfFrame();
        while (asyncOp == null || asyncOp.isDone == false)
        {
            Debug.Log(asyncOp.progress);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.ADVENTURE_SCENE));

        LoadScene(SceneNames.ADVENTURE_UI);
        yield return new WaitForEndOfFrame();
        while (asyncOp == null || asyncOp.isDone == false)
        {
            Debug.Log(asyncOp.progress);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        LoadingManager.Instance.OnLoadBarFull();
        GameManager.Instance.Initialized();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    public void UnloadScene(string sceneName)
    {
        StartCoroutine(UnLoadAsyncScene(sceneName));
    }

    private IEnumerator LoadAsyncScene(string sceneName)
    {
        if (sceneAsyncOperations.ContainsKey(sceneName) == true)
        {
            Debug.LogWarning("Scene already loaded " + sceneName);
            yield break;
        }

        Debug.Log("LOADING SCENE " + sceneName);
        var asyncLoading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        sceneAsyncOperations.Add(sceneName, asyncLoading);
        //while (!asyncLoading.isDone)
        //{

        //    Debug.Log(asyncLoading.progress + " + " + loadingProgress + " / " + (0.9f * additiveSceneList.Count));
        //    //loadingMeter.MeterValue = Mathf.Clamp01((asyncLoading.progress + loadingProgress) / (0.9f * additiveSceneList.Count));
        //    //Debug.Log(loadingMeter.MeterValue);
        //    yield return null;
        //}
    }

    private IEnumerator UnLoadAsyncScene(string sceneName)
    {
        var asyncUnloading = SceneManager.UnloadSceneAsync(sceneName);
        sceneAsyncOperations[sceneName] = asyncUnloading;
        yield return asyncUnloading;
        sceneAsyncOperations.Remove(sceneName);
    }

}
