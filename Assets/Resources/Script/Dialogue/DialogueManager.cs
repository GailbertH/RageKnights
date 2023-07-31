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
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private AddressableMode addressableMode = AddressableMode.LOCAL;

    [SerializeField] private TextMeshProUGUI dialogueNameLeft;
    [SerializeField] private TextMeshProUGUI dialogueNameRight;
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
    //Make a scriptable object to know the resource included to be loaded in it.

    private const string SPEAKER_TAG = "speaker";
    private const string LAYOUT_TAG = "layout";

    private const string LAYOUT_DIRECT_LEFT = "left";
    private const string LAYOUT_DIRECT_RIGHT = "right";

    private string leftSpeakerKey = string.Empty;
    private string rightSpeakerKey = string.Empty;

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
        //uiInput.actions["Click"].performed += Alert;
    }

    private void Start()
    {
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
        string speaker = "";
        string layout = "";
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
                    speaker = tagValue;
                    break;
                case LAYOUT_TAG:
                    layout = tagValue;
                    break;
                default:
                    Debug.LogError("Unregistered Tag " + tagKey);
                    break;
            }
        }

        if(speaker != "" && layout != "")
            ApplyTagChanges(speaker, layout);
    }

    private void ApplyTagChanges(string speaker, string layout)
    {
        if (layout == LAYOUT_DIRECT_LEFT)
        {
            ShowNewLeftSpeaker(speaker);
        }
        else if (layout == LAYOUT_DIRECT_RIGHT)
        {
            ShowNewRightSpeaker(speaker);
        }
    }

    private void ShowNewLeftSpeaker(string speakerKey)
    {
        if (leftSpeakerKey != speakerKey && speakerPortrait.ContainsKey(speakerKey))
        {
            if (dialoguePortraitLeftHolder.transform.childCount > 0)
            {
                //DestroySelf
                Destroy(dialoguePortraitLeftHolder.transform.GetChild(0).gameObject);
            }
            leftSpeakerKey = speakerKey;
            Instantiate<GameObject>(speakerPortrait[speakerKey], dialoguePortraitLeftHolder.transform);
        }
        dialogueNameLeft.text = speakerKey;
        dialogueNameObjLeft.SetActive(true);
        dialogueNameObjRight.SetActive(false);
        dialogueNameRight.text = "";
        dialoguePortraitLeftHolder.SetActive(true);
    }

    public void ShowNewRightSpeaker(string speakerKey)
    {
        if (rightSpeakerKey != speakerKey && speakerPortrait.ContainsKey(speakerKey))
        {
            if (dialogueProtraitRightHolder.transform.childCount > 0)
            {
                //DestroySelf
                Destroy(dialogueProtraitRightHolder.transform.GetChild(0).gameObject);
            }
            rightSpeakerKey = speakerKey;
            Instantiate<GameObject>(speakerPortrait[speakerKey], dialogueProtraitRightHolder.transform);
        }
        dialogueNameRight.text = speakerKey;
        dialogueNameObjRight.SetActive(true);
        dialogueNameObjLeft.SetActive(false);
        dialogueNameLeft.text = "";
        dialogueProtraitRightHolder.SetActive(true);
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
        StartDialogueMode(dialogueTextFile);
    }

    //private void Alert(InputAction.CallbackContext context)
    //{
    //    Debug.Log("ALERT");
    //}
}
