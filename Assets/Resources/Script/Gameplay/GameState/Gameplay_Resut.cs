using RageKnight;

public class Gameplay_Result : GameplayState_Base<GameplayState>
{
    public Gameplay_Result() : base(GameplayState.RESULT)
    {
    }

    public override void GameGoToNextState()
    {
        GameManager.Instance.StateMachine.SwitchState(GameplayState.EXIT);
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return (nextState == GameplayState.EXIT);
    }

    public override void GameStart()
    {
        var data = Manager.CombatTracker;
        GameUIManager.Instance.OpenPopup(PopupList.RESULT_UI, data, GameGoToNextState);
    }
}
