using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup splashCanvas;
    [SerializeField] private Text SetupText;
    [SerializeField] private Animation SetupTextAnimation;
    private bool isMenuReady = false;
    //should be handling load balancer
    void Start()
    {
        SetupText.text = "Loading...";
        SetupData();
    }

    private void SetupComplete()
    {
        SetupText.text = "Tap Screen To Start";
        SetupTextAnimation.Play();
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
    IEnumerator LoadStartupScenes(AsyncOperation loadScene)
    {
        yield return new WaitUntil(() => loadScene.isDone == true
        && LoadingManager.Instance != null
        && SceneTransitionManager.Instance != null);

        SceneTransitionManager.Instance.StartTransition(TransitionKey.SPLASH_TO_LOBBY, ShowMenu);
    }

    public void ShowMenu()
    {
        isMenuReady = true;
        SetupText.gameObject.SetActive(false);
        StartCoroutine(FadeOutCoroutine());
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
    private void UnloadScene()
    {
        SetupTextAnimation.Stop();
        splashCanvas = null;
        SetupText = null;
        SetupTextAnimation = null;
        SceneTransitionManager.Instance.UnloadScene(SceneNames.SPLASH_SCREEN);
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
