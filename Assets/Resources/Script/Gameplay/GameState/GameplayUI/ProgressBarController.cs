using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private Text stageText;
    [SerializeField] private Image stageIcon;
    [SerializeField] private Transform meter;

    public void InitializeProgressBar(string stageName)
    {
        stageText.text = stageName;
        ResetValues();
        this.gameObject.SetActive(true);
    }

    public void ResetValues()
    {
        UpdateProgress(0);
    }

    public void UpdateProgress(float currentValue)
    {
        if (currentValue < 0 || currentValue > 1)
        {
            currentValue = currentValue < 0 ? 0 : 1;
        }
        meter.localScale = new Vector3(currentValue, 1, 1);
    }
}
