using RageKnight;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarHandler : MonoBehaviour
{
    [SerializeField] private Transform progressGroup;
    [SerializeField] private RectTransform progressGroupRect;
    [SerializeField] private GameObject progressPanel;

    private List<ProgressBarController> stageList = new List<ProgressBarController>();
    private ProgressBarController currentStage = null;
    private bool isInitialized = false;
    private int stageTracker = 0;

    public bool IsInitialized
    {
        get { return IsInitialized; }
    }

    public void Initialize()
    {
        int stageCount = GameManager.Instance.StageCount;
        stageTracker = GameManager.Instance.StageTracker;

        for (int i = 0; i < stageCount; i++)
        {
            GameObject newPanel = Instantiate<GameObject>(progressPanel, progressGroup) as GameObject;
            ProgressBarController progressBar = newPanel.GetComponent<ProgressBarController>();
            progressBar.InitializeProgressBar("1-" + (i + 1));
            stageList.Add(progressBar);
            if (currentStage == null)
            {
                currentStage = stageList[stageTracker];
            }
        }
        isInitialized = true;
    }

    public void ResetCurrentStageState()
    {
        currentStage.ResetValues();
    }

    public void UpdateStage(int stageNumber)
    {
        if (stageNumber < stageList.Count)
        {
            Vector3 currentPosition = progressGroupRect.localPosition;
            progressGroupRect.localPosition = new Vector3(currentPosition.x - 250, currentPosition.y, currentPosition.z);
            currentStage = stageList[stageNumber];
        }
    }

    public void SetUIActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    public void UpdateStageProgress(float progressValue)
    {
        currentStage.UpdateProgress(progressValue);
    }
}
