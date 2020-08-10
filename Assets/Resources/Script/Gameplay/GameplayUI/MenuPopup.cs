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
        GameManager.Instance.StateMachine.GetCurrentState.GoToNextState();
        CloseButton();
    }
}
