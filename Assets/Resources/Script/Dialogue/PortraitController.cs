using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitController : MonoBehaviour
{
    [SerializeField] private GameObject portraitImage;
    [SerializeField] private Animation genericAnimation;
    [SerializeField] private Animation uniqueAnimation;

    private void Awake()
    {
        portraitImage.SetActive(false);
    }

    public void PlayGenericAnimation(AnimationClip clip, string animationName)
    {
        portraitImage.SetActive(true);
        if (clip != null)
        {
            if (genericAnimation.GetClip(animationName) == null)
            {
                genericAnimation.AddClip(clip, animationName);
            }
            genericAnimation.Play(animationName);
        }
    }

    public void PlayUniqueAnimation(string animationName)
    {
        uniqueAnimation.Play(animationName);
    }

    public void HideDestroy(AnimationClip clip, string animationName)
    {
        PlayGenericAnimation(clip, animationName);
        Destroy(this.gameObject, 0.5f);
    }
    public void Destroy(string animation)
    {
        Destroy(this.gameObject);
    }
}
