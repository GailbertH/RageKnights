/*using RageKnight;
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
             nextState == GameplayState.ADVENTURE ||
            nextState == GameplayState.RESULT);
    }

    public override void GameStart()
    {
        base.GameStart();
        GameUIManager.Instance.ShowRageImage();
        Manager.EnemyHandler.SetupRageMode();
        stateSwitch_TEst = 5;

        GameUIManager.Instance.UpdateControlMode(State);
    }

    public override void GameUpdate()
    {
        if (Manager != null)
        {
            Manager.PlayerHandler.PlayerMoveForward();
            MoveEnvironment(GameManager.RUN_SPEED);
            Manager.EnemyHandler.MoveUnits(GameManager.RUN_SPEED);
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
            nextState = GameplayState.COMBAT;
            if (Manager.EnemyHandler.HasSoldiers == false)
            {
                UnityEngine.Debug.Log("Is enemy Alive? " + Manager.EnemyHandler.HasSoldiers);
                Manager.IncrementStage();
                nextState = Manager.IsFinalStage ? GameplayState.RESULT : GameplayState.ADVENTURE;
            }
            Handler.SwitchState(nextState);
        }
    }

    public override void GameEnd()
    {
        GameUIManager.Instance.ShowRageImage();
        Manager.EnemyHandler.EndRageMode();
    }
}
*/