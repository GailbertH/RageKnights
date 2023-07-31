using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
	[SerializeField] private LoadingMeter loadingMeter;
	[SerializeField] GameObject canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] Camera mainCamera;
    [SerializeField] Image loadingScreenCombat;

    private static LoadingManager instance;
	public static LoadingManager Instance { get { return instance; } }

	void Awake()
	{
		instance = this;
		mainCamera.gameObject.SetActive (true);
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
        loadingScreenCombat.sprite = null;
        loadingScreenCombat.gameObject.SetActive(false);
        StartCoroutine(FadeOutCoroutine());
    }

    public void ShowLoading()
    {

        CanvasVisible(true);

        StartCoroutine(CaptureScreen());

        this.SetUpLoadingMeter();
        loadingMeter.Reset();
    }

    private void CanvasVisible(bool toShow)
    {
        //canvasGroup.alpha = toShow ? 1 : 0;
        canvas.SetActive(toShow);
    }


    private IEnumerator CaptureScreen()
    {
        int width = Screen.width;
        int height = Screen.height;
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        loadingScreenCombat.sprite = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        loadingScreenCombat.gameObject.SetActive(true);
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
}
