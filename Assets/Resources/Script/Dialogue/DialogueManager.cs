using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets.ResourceLocators;
using System.Linq;
//using UnityEngine.InputSystem;

public enum AddressableMode
{
    LOCAL, REMOTE
}
public class DA_GenericAnim
{
    public const string FADE_IN = "DA_FadeIn";
    public const string FLINCH = "DA_Flinch";
    public const string HOP = "DA_Hop";
    public const string ATTACKS_LEFT = "DA_AttacksLeft";
    public const string ATTACKS_RIGHT = "DA_AttacksRight";
}

public class DialogueTags
{
    public string speaker = string.Empty;
    public string layout = string.Empty;
    public string genericAnim = string.Empty;
    public string otherAnim = string.Empty;
}
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private AddressableMode addressableMode = AddressableMode.LOCAL;

    [SerializeField] private TextMeshProUGUI dialogueNameLeft;
    [SerializeField] private TextMeshProUGUI dialogueNameRight;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject dialogueNameObjLeft;
    [SerializeField] private GameObject dialogueNameObjRight;
    [SerializeField] private GameObject dialoguePortraitLeftHolder;
    [SerializeField] private GameObject dialogueProtraitRightHolder;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private GameObject ChoiceHolder;
    [SerializeField] private List<DialogueChoice> choices;

    //Temporary
    [SerializeField] private AssetReference assetRef;
    [SerializeField] private List<string> resourceToLoad;
    [SerializeField] private List<AnimationClip> genericAnimations;
    //Make a scriptable object to know the resource included to be loaded in it.

    private const string SPEAKER_TAG = "speaker";
    private const string LAYOUT_TAG = "layout";
    private const string GENERIC_ANIM_TAG = "genericAnim";
    private const string OTHER_ANIM_TAG = "otherGenAnim";

    private const string LAYOUT_DIRECT_LEFT = "left";
    private const string LAYOUT_DIRECT_RIGHT = "right";

    private string leftSpeakerKey = string.Empty;
    private string rightSpeakerKey = string.Empty;
    private PortraitController leftPortrait;
    private PortraitController rightPortrait;

    private Dictionary<string, GameObject> speakerPortrait = new Dictionary<string, GameObject>();
    //[SerializeField] private PlayerInput uiInput;
    private Story currentStory;
    private Coroutine dialogueCoroutine;
    private string currentDialogue;
    private TextAsset dialogueTextFile;


    private static DialogueManager instance = null;
    public static DialogueManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        dialogueBox.SetActive(false);
        //uiInput.actions["Click"].performed += Alert;
    }

    private void Start()
    {
        foreach (var clips in genericAnimations)
        {
            Debug.Log(clips.name);
        }
        Addressables.ClearResourceLocators();

        if (addressableMode == AddressableMode.LOCAL)
        {
            StartCoroutine(LoadAddressables());
        }
        else
        {
            StartCoroutine(DownloadOrLoadAddressables());
        }
    }

    public void OnClick()
    {
        if (dialogueCoroutine != null)
        {
            dialogueText.text = currentDialogue;
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }
        else
            ContinueStory();
    }

    public void StartDialogueMode(TextAsset textJson)
    {
        dialogueBox.SetActive(true);
        currentStory = new Story(textJson.text);
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            currentDialogue = currentStory.Continue();
            currentDialogue = currentDialogue.Replace("<player>", "Gail");
            SetupTag(currentStory.currentTags);
            if (ShowChoices())
            {
                dialogueText.text = currentDialogue;
            }
            else
            {
                dialogueCoroutine = StartCoroutine(ShowDialogue(currentDialogue));
            }
        }
        else
        {
            EndDialogueMode();
        }
    }

    public void SetupTag(List<string> tagList)
    {
        DialogueTags dialogueTags = new DialogueTags();
        for (int i = 0; i < tagList.Count; i++)
        {
            string[] splitTag = tagList[i].Split(':');
            if (splitTag.Length <= 1)
            {
                Debug.LogError("Tag Error " + tagList[i]);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    dialogueTags.speaker = tagValue;
                    break;
                case LAYOUT_TAG:
                    dialogueTags.layout = tagValue;
                    break;
                case GENERIC_ANIM_TAG:
                    dialogueTags.genericAnim = tagValue;
                    break;
                case OTHER_ANIM_TAG:
                    dialogueTags.otherAnim = tagValue;
                    break;
                default:
                    Debug.LogWarning("Unregistered Tag " + tagKey);
                    break;
            }
        }

        ApplyTagChanges(dialogueTags);
    }

    private void ApplyTagChanges(DialogueTags dialogueTags)
    {
        if (dialogueTags.layout == LAYOUT_DIRECT_LEFT)
        {
            ShowNewLeftSpeaker(dialogueTags);
        }
        else if (dialogueTags.layout == LAYOUT_DIRECT_RIGHT)
        {
            ShowNewRightSpeaker(dialogueTags);
        }
    }

    private void ShowNewLeftSpeaker(DialogueTags dialogueTags)
    {
        if (leftSpeakerKey != dialogueTags.speaker && speakerPortrait.ContainsKey(dialogueTags.speaker))
        {
            if (dialoguePortraitLeftHolder.transform.childCount > 0)
            {
                //DestroySelf
                leftPortrait = null;
                Destroy(dialoguePortraitLeftHolder.transform.GetChild(0).gameObject);
            }
            leftSpeakerKey = dialogueTags.speaker;
            var leftObj = Instantiate<GameObject>(speakerPortrait[dialogueTags.speaker], dialoguePortraitLeftHolder.transform);
            leftPortrait = leftObj.GetComponent<PortraitController>();
        }

        dialogueNameLeft.text = dialogueTags.speaker;
        PlayLeftGenAnimation(dialogueTags.genericAnim);
        PlayRightAnimation(dialogueTags.otherAnim);
        dialogueNameObjLeft.SetActive(true);
        dialogueNameObjRight.SetActive(false);
        dialogueNameRight.text = "";
        dialoguePortraitLeftHolder.SetActive(true);
    }

    private void ShowNewRightSpeaker(DialogueTags dialogueTags)
    {
        if (rightSpeakerKey != dialogueTags.speaker && speakerPortrait.ContainsKey(dialogueTags.speaker))
        {
            if (dialogueProtraitRightHolder.transform.childCount > 0)
            {
                //DestroySelf
                rightPortrait = null;
                Destroy(dialogueProtraitRightHolder.transform.GetChild(0).gameObject);
            }
            rightSpeakerKey = dialogueTags.speaker;
            var rightObj = Instantiate<GameObject>(speakerPortrait[dialogueTags.speaker], dialogueProtraitRightHolder.transform);
            rightPortrait = rightObj.GetComponent<PortraitController>();
        }
        dialogueNameRight.text = dialogueTags.speaker;
        PlayLeftGenAnimation(dialogueTags.otherAnim);
        PlayRightAnimation(dialogueTags.genericAnim);
        dialogueNameObjRight.SetActive(true);
        dialogueNameObjLeft.SetActive(false);
        dialogueNameLeft.text = "";
        dialogueProtraitRightHolder.SetActive(true);
    }

    private void PlayLeftGenAnimation(string genericAnimation)
    {
        if (genericAnimation != string.Empty)
        {
            var clip = genericAnimations.FirstOrDefault(x => x.name == genericAnimation);
            leftPortrait.PlayGenericAnimation(clip, genericAnimation);
        }
    }

    private void PlayRightAnimation(string genericAnimation)
    {
        if (genericAnimation != string.Empty)
        {
            var clip = genericAnimations.FirstOrDefault(x => x.name == genericAnimation);
            rightPortrait.PlayGenericAnimation(clip, genericAnimation);
        }
    }

    public bool ShowChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        for (int i = 0; i < currentChoices.Count; i++)
        {
            choices[i].Setup(currentChoices[i].text, i);
        }
        if (currentChoices.Count > 0)
        {
            ChoiceHolder.SetActive(true);
            StartCoroutine(SetSelectFirstChoice());
            return true;
        }
        return false;
    }

    public void ChoiceSelected(int index)
    {
        currentStory.ChooseChoiceIndex(index);
        ChoiceHolder.SetActive(false);
        ContinueStory();
    }

    public void EndDialogueMode()
    {
        dialogueNameLeft.text = "";
        dialogueNameRight.text = "";
        dialogueText.text = "";
        currentDialogue = "";
        SceneTransitionManager.Instance.StartTransition(TransitionKey.DIALOGUE_TO_ADVENTURE);
        //End Dialogue Mode
    }

    private IEnumerator SetSelectFirstChoice()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(choices[0].gameObject);
    }

    private IEnumerator ShowDialogue(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        dialogueCoroutine = null;
    }


    private IEnumerator LoadAddressables()
    {
        bool textReady = false;
        bool portraitsReady = false;
        assetRef.LoadAssetAsync<TextAsset>().Completed +=
        (asyncOp) =>
        {
            if (asyncOp.Status == AsyncOperationStatus.Succeeded)
            {
                dialogueTextFile = asyncOp.Result;
            }
            else
            {
                Debug.Log("Asset Loading Failed");
            }
            textReady = true;
        };

        yield return new WaitForEndOfFrame();

        foreach (var resource in resourceToLoad)
        {
            Addressables.LoadAssetsAsync<UnityEngine.Object>(resource, obj =>
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    var speakerObj = (GameObject)obj;

                    speakerPortrait.Add(resource, speakerObj);
                }
            }).Completed += (asynOp) =>
            {
                Debug.Log(resource);
                if (resource == resourceToLoad.Last())
                {
                    portraitsReady = true;
                }
            };
        }

        yield return new WaitUntil(() => textReady && portraitsReady);
        yield return new WaitForSeconds(1);
        StartDialogueMode(dialogueTextFile);
    }

    private IEnumerator DownloadOrLoadAddressables()
    {
        var allLocations = new List<IResourceLocation>();

        var loadop = Addressables.LoadResourceLocationsAsync("RemoteResourceTesting");
        Debug.Log(loadop.Status == AsyncOperationStatus.None);
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync("RemoteResourceTesting");
        yield return new WaitUntil(() => getDownloadSize.IsDone);
        Debug.Log("Downloadsize result: " + getDownloadSize.Result + " kb");
        if (getDownloadSize.Result > 0)
        {
            //Debug.Log("Downloading");
            AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync("RemoteResourceTesting");
            while (!downloadDependencies.IsDone)
            {
                var totalBytes = downloadDependencies.GetDownloadStatus().DownloadedBytes;
                //Debug.Log("Total Bytes " + totalBytes);
                var _percent = downloadDependencies.GetDownloadStatus().Percent;
                //Debug.Log(_percent);
                yield return null;
            }
            downloadDependencies.WaitForCompletion();
            Debug.Log("Download finish");
        }

        loadop = Addressables.LoadResourceLocationsAsync("RemoteResourceTesting");
        Debug.Log("loadop is done " + loadop.IsDone);

        Debug.Log(loadop.Result == null);
        if (loadop.Status == AsyncOperationStatus.Succeeded)
        {
            AsyncOperationHandle<TextAsset> loadText = Addressables.LoadAssetAsync<TextAsset>("RemoteResourceTesting");
            dialogueTextFile = loadText.WaitForCompletion();
        }

        //AsyncOperationHandle<TextAsset> loadText = Addressables.LoadAssetAsync<TextAsset>("RemoteResourceTesting");
        //dialogueTextFile = loadText.WaitForCompletion();


        //Addressables.LoadAssetsAsync<UnityEngine.Object>("RemoteResourceTesting", obj =>
        //{
        //    Debug.Log("Files Unity Object = " + obj.GetType());
        //});
        //Addressables.LoadAssetsAsync<object>("RemoteResourceTesting", obj =>
        //{
        //    Debug.Log("Files = " + obj);
        //});

        //const string key = "TestingImage";
        //Addressables.LoadAssetsAsync<Texture2D>(key, obj =>
        //{
        //    Debug.Log("Files = " + obj.name);
        //});

        Addressables.Release(loadop);
        yield return new WaitForSeconds(1);
        StartDialogueMode(dialogueTextFile);
    }

    //private void Alert(InputAction.CallbackContext context)
    //{
    //    Debug.Log("ALERT");
    //}
}
