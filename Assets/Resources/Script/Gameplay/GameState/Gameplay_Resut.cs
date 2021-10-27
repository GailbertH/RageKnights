using RageKnight;
using RageKnight.GameState;
using UnityEngine;

public class Gameplay_Result : GameplayState_Base<GameplayState>
{
    GameUIManager Controls = null;

    public Gameplay_Result(GameManager manager, RageKnight_InGame handler) : base(GameplayState.RESULT, manager, handler)
    {
    }

    public override void GameGoToNextState()
    {
        Handler.SwitchState(GameplayState.EXIT);
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return (nextState == GameplayState.EXIT);
    }

    public override void GameStart()
    {
        Controls = Manager.GameUIManager;
        var data = Manager.CombatTracker;
        Controls.OpenPopup(PopupList.RESULT_UI, data, GameGoToNextState);
    }
}
