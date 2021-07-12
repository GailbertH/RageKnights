using RageKnight;
using RageKnight.GameState;
using System;

public class Gameplay_Rage : GameplayState_Base<GameplayState>
{
    private GameplayState nextState = GameplayState.COMBAT;
    private int stateSwitch_TEst = 5;

    public Gameplay_Rage(GameManager manager, RageKnight_InGame handler) : base(GameplayState.RAGE, manager, handler)
    {
    }

    public override void GameGoToNextState()
    {
        Handler.SwitchState(nextState);
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return (nextState == GameplayState.COMBAT ||
            nextState == GameplayState.RESULT);
    }

    public override void GameStart()
    {
        base.GameStart();
        Manager.GameUIManager.ShowRageImage();
        Manager.PlayerHandler.SetupRageMode();
        Manager.EnemyHandler.SetupRageMode();
        Manager.PlayerHandler?.PlayerRageActivate();
        stateSwitch_TEst = 5;

        Manager.GameUIManager.UpdateControlMode(State);
    }

    private float WALK_SPEED = -0.15f;
    private float WALK_SPEED_ENEMY = -0.15f;
    public override void GameUpdate()
    {
        if (Manager != null)
        {
            Manager.PlayerHandler.PlayerMoveForward();
            MoveEnvironment(WALK_SPEED);
            Manager.EnemyHandler.MoveUnits(WALK_SPEED_ENEMY);
            Manager.PlayerHandler?.PlayerArmyAttack(Manager);
        }
    }

    private void MoveEnvironment(float speed)
    {
        if (Manager.EnvironmentHandler != null)
        {
            Manager.EnvironmentHandler.MoveEnvironment(speed);
        }
    }

    public override void GameTimerUpdate()
    {
        stateSwitch_TEst--;
        if (stateSwitch_TEst <= 0)
        {
            Handler.SwitchState(nextState);
        }
    }

    public override void GameEnd()
    {
        Manager.GameUIManager.ShowRageImage();
        Manager.PlayerHandler.EndRageMode();
        Manager.EnemyHandler.EndRageMode();
    }
}
