using RageKnight;
using RageKnight.GameState;
using UnityEngine;

public class Gameplay_Exit : GameplayState_Base<GameplayState>
{
    public Gameplay_Exit(GameManager manager, RageKnight_InGame handler) : base(GameplayState.EXIT, manager, handler)
    {
    }

    public override void GameGoToNextState()
    {
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return false;
    }

    public override void GameStart()
    {
        Handler.GoToNextState();
    }
}
