using RageKnight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPopup : PopupBase
{
    public void Resume()
    {
        CloseButton();
    }

    public void Quit()
    {
        //Make sure this is exit state
        GameManager.Instance.ReturnToTitleScreen();
        CloseButton();
    }
}
