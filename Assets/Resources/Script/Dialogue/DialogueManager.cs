using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueNameLeft;
    [SerializeField] private TextMeshProUGUI dialogueNameRight;
    [SerializeField] private GameObject dialogueNameObjLeft;
    [SerializeField] private GameObject dialogueNameObjRight;
    [SerializeField] private GameObject dialoguePortraitLeft;
    [SerializeField] private GameObject dialogueProtraitRight;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextAsset dialogueTextFile;
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private GameObject ChoiceHolder;
    [SerializeField] private List<DialogueChoice> choices;

    private const string SPEAKER_TAG = "speaker";
    private const string LAYOUT_TAG = "layout";

    private const string LAYOUT_DIRECT_LEFT = "left";
    private const string LAYOUT_DIRECT_RIGHT = "right";

    //[SerializeField] private PlayerInput uiInput;
    private Story currentStory;
    private Coroutine dialogueCoroutine;
    private string currentDialogue;

    private static DialogueManager instance = null;
    public static DialogueManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        StartDialogueMode(dialogueTextFile);
        //uiInput.actions["Click"].performed += Alert;
    }

    //private void Alert(InputAction.CallbackContext context)
    //{
    //    Debug.Log("ALERT");
    //}

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
            dialogueNameLeft.text = speaker;
            dialogueNameObjLeft.SetActive(true);
            dialogueNameObjRight.SetActive(false);
            dialogueNameRight.text = "";
            dialoguePortraitLeft.SetActive(true);
        }
        else if (layout == LAYOUT_DIRECT_RIGHT)
        {
            dialogueNameRight.text = speaker;
            dialogueNameObjRight.SetActive(true);
            dialogueNameObjLeft.SetActive(false);
            dialogueNameLeft.text = "";
            dialogueProtraitRight.SetActive(true);
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
}
