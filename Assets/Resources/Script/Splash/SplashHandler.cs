using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup splashCanvas;
    [SerializeField] private Button StartButton;
    [SerializeField] private Text SetupText;
    [SerializeField] private Animation SetupTextAnimation;
    private bool isMenuReady = false;
    //should be handling load balancer
    void Start()
    {
        SetupText.text = "Loading...";
        StartButton.gameObject.SetActive(false);
        SetupData();
    }

    private void SetupComplete()
    {
        SetupText.text = "Tap Screen To Start";
        SetupTextAnimation.Play();
        StartButton.gameObject.SetActive(true);
    }

    private void SetupData()
    {
        StartCoroutine(FakeLoadBalancer());
        SetupLoadManager();
    }

    private void SetupLoadManager()
    {
        var loadSceneAsync = SceneManager.LoadSceneAsync(SceneNames.LOADING_SCREEN, LoadSceneMode.Additive);
        StartCoroutine(LoadStartupScenes(loadSceneAsync));
    }

    public void OnStartButtonPress()
    {
        SetupText.gameObject.SetActive(false);
        StartButton.interactable = false;
        StartCoroutine(FadeOutCoroutine());
        LoadingManager.Instance.ActivateSilentLoadedScene();
    }

    private void UnloadScene()
    {
        SetupTextAnimation.Stop();
        splashCanvas = null;
        StartButton = null;
        SetupText = null;
        SetupTextAnimation = null;
        LoadingManager.Instance.SetSceneToUnload(SceneNames.SPLASH_SCREEN);
        LoadingManager.Instance.UnloadScene();
    }

    IEnumerator LoadStartupScenes(AsyncOperation loadScene)
    {
        yield return new WaitUntil(() => loadScene.isDone == true && LoadingManager.Instance != null);

        LoadingManager.Instance.SetSceneToLoad(SceneNames.DATA_SCENE);
        LoadingManager.Instance.LoadScene();
        LoadingManager.Instance.SetSceneToLoad(SceneNames.LOBBY_SCENE);
        LoadingManager.Instance.SilentLoadScene();
        isMenuReady = true;
    }

    IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        float time = 0.5f;
        while (splashCanvas.alpha > 0)
        {
            splashCanvas.alpha -= Time.deltaTime / time;
            yield return null;
        }
        UnloadScene();
    }

    IEnumerator FakeLoadBalancer()
    {

#if UNITY_EDITOR == false
        SetupText.text = "Retrieving Data...";
        yield return new WaitForSeconds(1f);
        SetupText.text = "Populating Data...";
        yield return new WaitForSeconds(1f);
        SetupText.text = "Chiong Engot...";
        yield return new WaitForSeconds(1.5f);
        SetupText.text = "Jess BOBO!!!";
        yield return new WaitForSeconds(1.5f);
        SetupText.text = "Tangina mo Ian!!!";
        yield return new WaitForSeconds(1.5f);
        SetupText.text = "Gail so awesome <3";
        yield return new WaitForSeconds(1.5f);
        SetupText.text = "Almost There..";
#endif
        while (!isMenuReady)
        {
            yield return null;
        }

        SetupComplete();
    }
}
