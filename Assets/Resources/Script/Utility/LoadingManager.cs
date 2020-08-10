using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
	[SerializeField] private LoadingMeter loadingMeter;
	[SerializeField] GameObject canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] Camera mainCamera;
    [SerializeField] int gameSpeed = 60;

	private AsyncOperation asyncLoading;
	private AsyncOperation asyncUnloading;
	private string sceneToLoad;
	private string sceneToUnload;
	private Coroutine loadingRoutine;
	private Coroutine unloadingRoutine;

	private static LoadingManager instance;
	public static LoadingManager Instance { get { return instance; } }

	void Awake()
	{
		instance = this;
		mainCamera.gameObject.SetActive (true);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        CanvasVisible(false);
    }

    private void SetUpLoadingMeter(bool autoLoadDone = true)
	{
		mainCamera.gameObject.SetActive (true);
        loadingMeter.OnLoadMeterChange (this.OnLoadBarChange);
        if (autoLoadDone == true)
        {
            loadingMeter.OnLoadDone(this.OnLoadBarFull);
        }
	}

	private void OnLoadBarChange(float value)
	{
		Debug.Log("LoadBar " + value);
	}

	public void OnLoadBarFull()
	{
		Debug.Log("Load Bar Full ");

		asyncLoading.allowSceneActivation = true;

		if(loadingRoutine != null)
			StopCoroutine (loadingRoutine);

		if(unloadingRoutine != null)
			StopCoroutine (unloadingRoutine);

        StartCoroutine(FadeOutCoroutine());
    }

	#region SceneManagement
	public void SetSceneToLoad(string sceneName)
	{
		sceneToLoad = sceneName;
	}

	public void SetSceneToUnload(string sceneName)
	{
		sceneToUnload = sceneName;
	}

    /// <summary>
    /// Neeeds to call SetSceneToLoad first
    /// </summary>
    public void LoadScene()
    {
        loadingRoutine = StartCoroutine(LoadAsynceScene(true));
    }

    /// <summary>
    /// Neeeds to call SetSceneToLoad first this is background load version set is not visible first
    /// </summary>
    public void SilentLoadScene()
    {
        loadingRoutine = StartCoroutine(LoadAsynceScene(false));
    }

    public void ActivateSilentLoadedScene()
    {
        asyncLoading.allowSceneActivation = true;
    }

    public void UnloadScene()
    {
        unloadingRoutine = StartCoroutine(UnLoadAsyncScene());
    }

    public void LoadGameScene()
	{
        CanvasVisible(true);
        this.SetUpLoadingMeter ();
		loadingMeter.Reset ();
		unloadingRoutine = StartCoroutine (UnLoadAsyncScene ());
		loadingRoutine = StartCoroutine (LoadAsynceScene(true));
	}

    private void CanvasVisible(bool toShow)
    {
        canvasGroup.alpha = toShow ? 1 : 0;
        canvas.SetActive(toShow);
    }

	private IEnumerator LoadAsynceScene(bool allowActivation)
	{
		string[] sceneToLoadQueue = this.sceneToLoad.Split (',');
		float loadingProgress = 0;
		for (int i = 0; sceneToLoadQueue.Length > i; i++) 
		{
			loadingProgress = i;
			Debug.Log ("LOADING SCENE " + sceneToLoadQueue [i]);
			asyncLoading = SceneManager.LoadSceneAsync (sceneToLoadQueue[i], LoadSceneMode.Additive);
			asyncLoading.allowSceneActivation = allowActivation;

			while (!asyncLoading.isDone) 
			{
				//Debug.Log(asyncLoading.progress + " + " + loadingProgress + " / " + (0.9f * sceneToLoadQueue.Length));
				loadingMeter.MeterValue = Mathf.Clamp01 ((asyncLoading.progress + loadingProgress) / (0.9f * sceneToLoadQueue.Length));
				//Debug.Log (loadingMeter.MeterValue);
				yield return null;
			}
		}
		sceneToLoad = "";
	}

	private IEnumerator UnLoadAsyncScene()
	{
		if (sceneToUnload != "") 
		{
			string[] sceneToUnloadQueue = this.sceneToUnload.Split (',');
			for (int i = 0; sceneToUnloadQueue.Length > i; i++) 
			{
				asyncUnloading = SceneManager.UnloadSceneAsync (sceneToUnloadQueue[i]);

				while (!asyncUnloading.isDone) 
				{
					yield return null;
				}
			}
			sceneToUnload = "";
		}
	}

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        float time = 0.5f;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        CanvasVisible(false);
        mainCamera.gameObject.SetActive(false);
    }
    #endregion
}
