using RageKnight;
using RageKnight.GameState;
using UnityEngine;

public class Gameplay_Adventure : GameplayState_Base<GameplayState>
{
    private float WALK_SPEED = -0.1f;
    private float ENEMY_WALK_SPEED = -0.1f;
    private bool HasEnemySpawn = false;

    public Gameplay_Adventure(GameManager manager, RageKnight_InGame handler) : base (GameplayState.ADVENTURE, manager, handler)
    {
        WALK_SPEED = Application.targetFrameRate > 30 ? -0.05f : -0.1f;
        ENEMY_WALK_SPEED = Application.targetFrameRate > 30 ? -0.05f : -0.1f;
    }

    public override void GameGoToNextState()
    {
        Handler.SwitchState(GameplayState.COMBAT);
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return (nextState == GameplayState.COMBAT);
    }

    public override void GameStart()
    {
        GameUIManager Controls = Manager.GameUIManager;
        Controls?.UpdateControlMode(State);
        Controls?.UpdateMiddleUIModle(State);
        Manager.EnemyHandler.Initialize();
        Manager.AddGold(0);
        HasEnemySpawn = false;
    }

    public override void GameUpdate()
    {
        if (Manager != null)
        {
            Manager.PlayerHandler.PlayerMoveForward();
            MoveEnvironment(WALK_SPEED);
            if (HasEnemySpawn == true)
            {
                if (Manager.EnemyHandler.IsEnemyInBattlePosition(ENEMY_WALK_SPEED, Manager))
                {
                    GameGoToNextState();
                }
            }
        }
    }

    public override void GameTimerUpdate()
    {
        CheckIfEnemySpawned();
    }

    #region private methods
    private void CheckIfEnemySpawned()
    {
        if (HasEnemySpawn == true)
        {
            return;
        }

        if (Manager.EnemyHandler != null)
        {
            HasEnemySpawn = Manager.EnemyHandler.IsEnemySpawn();
        }
    }

    private void MoveEnvironment(float speed)
    {
        if (Manager.EnvironmentHandler != null)
        {
            Manager.EnvironmentHandler.MoveEnvironment(speed);
        }
    }
    #endregion
}
