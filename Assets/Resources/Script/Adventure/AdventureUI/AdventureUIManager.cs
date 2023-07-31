using RageKnight;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AdventureUIManager : MonoBehaviour
{
    [SerializeField] private GameJoyStick gameJoyStick;
    private static AdventureUIManager instance = null;
    public static AdventureUIManager Instance { get { return instance; } }
    private float GetJoyStickHorizontal { get { return gameJoyStick.Horizontal(); } }
    private float GetJoyStickVertical { get { return gameJoyStick.Vertical(); } }
    private bool isDragging = false;
    public bool OnJoyStickDrag
    {
        set { isDragging = value; }
        get { return isDragging; }
    }

    public Vector2 GetMovement
    {
        get 
        {
            if (isDragging)
                return new Vector2(GetJoyStickHorizontal, GetJoyStickVertical);
            else
                return Vector2.zero;
        }
    }

    private void Awake()
    {
        instance = this;

#if UNITY_ANDROID
        gameJoyStick.Initialize();
        gameJoyStick.gameObject.SetActive(true);
#else
        gameJoyStick.gameObject.SetActive(false);
#endif
    }

    //#if UNITY_EDITOR
    //    void FixedUpdate()
    //    {
    //        if (gameJoyStick.GetInputVector != Vector3.zero)
    //            return;

    //        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
    //        {
    //            isDragging = true;
    //        }
    //        else if (isDragging)
    //        {
    //            isDragging = false;
    //        }
    //    }
    //#endif
}
