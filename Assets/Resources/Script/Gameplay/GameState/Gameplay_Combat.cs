using RageKnight;
using RageKnight.GameState;
using System;
using System.Collections.Generic;

public class Gameplay_Combat : GameplayState_Base<GameplayState>
{
    private bool startCountDown = false;
    private Action onStateEndAction = null;
    CombatStateMachine combatStateMachine = null;

    public Gameplay_Combat() : base(GameplayState.COMBAT)
    {
        combatStateMachine = new CombatStateMachine(manager, this);
    }

    private GameplayState nextState = GameplayState.RESULT;

    public override void GameGoToNextState()
    {
        Handler.SwitchState(nextState);
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return (nextState == GameplayState.RESULT);
    }

    public override void GameStart()
    {
        base.GameStart();
        startCountDown = false;

        Manager.PlayerHandler.PlayerResetAnimation();

        GameUIManager.Instance.UpdateControlMode(State);
        GameUIManager.Instance.UpdateMiddleUIModle(State);
        combatStateMachine.Start();
    }

    public override void GameUpdate()
    {
        combatStateMachine.Update();
    }

    public override void GameTimerUpdate()
    {
        combatStateMachine.TimerUpdate();
    }

    public void NextState()
    {
        Handler.SwitchState(GameplayState.RESULT);
    }

    public override void GameEnd()
    {
    }
}
