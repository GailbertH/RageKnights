using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueChoiceText;
    [SerializeField] private Button dialogueButton;
    private int assignIndex;

    public void Setup(string content, int index)
    {
        assignIndex = index;
        dialogueChoiceText.text = content;
        dialogueButton.interactable = true;
    }

    public void OnClick()
    {
        dialogueButton.interactable = false;
        DialogueManager.Instance.ChoiceSelected(assignIndex);
    }
}
