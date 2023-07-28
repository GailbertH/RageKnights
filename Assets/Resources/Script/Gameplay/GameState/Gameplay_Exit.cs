using RageKnight;

public class Gameplay_Exit : GameplayState_Base<GameplayState>
{
    public Gameplay_Exit() : base(GameplayState.EXIT)
    {
    }

    public override void GameStart()
    {
        base.GameStart();
        GameGoToNextState();
    }
    public override void GameGoToNextState()
    {
        GameManager.Instance.StateMachine.FinishState();
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return false;
    }

    public override void GameEnd()
    {
        base.GameEnd();
        GameManager.Instance.ReturnBackToAdventure();
    }
}
