using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPopup : PopupBase
{
    public void SelectStage(int stageNumber)
    {
        LobbyManager.Instance.SetupGameScene(stageNumber, "00001");
    }
}
