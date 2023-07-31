using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image bgImg;
    [SerializeField] private Image joyStickImage;
    private AdventureUIManager gameController = null;
    private Vector3 inputVector;

    public void Initialize()
    {
        gameController = AdventureUIManager.Instance;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform
            , ped.position
            , ped.pressEventCamera
            , out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2f, 0f, pos.y * 2f);
            inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

            joyStickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3)
                , inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        if (gameController != null)
        {
            OnDrag(ped);
            gameController.OnJoyStickDrag = true;
        }
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        if (gameController != null)
        {
            inputVector = Vector3.zero;
            joyStickImage.rectTransform.anchoredPosition = inputVector;
            gameController.OnJoyStickDrag = false;
        }
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.z;
    }

#if UNITY_EDITOR
    public Vector3 GetInputVector { get { return inputVector; } }
#endif
}
