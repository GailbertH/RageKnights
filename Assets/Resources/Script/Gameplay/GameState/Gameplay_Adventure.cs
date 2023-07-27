/*
using RageKnight;
using RageKnight.GameState;
using UnityEngine;

public class Gameplay_Adventure : GameplayState_Base<GameplayState>
{
    private bool HasEnemySpawn = false;

    public Gameplay_Adventure(GameManager manager, RageKnight_InGame handler) : base (GameplayState.ADVENTURE, manager, handler)
    {
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
        base.GameStart();
        GameUIManager.Instance.UpdateControlMode(State);
        GameUIManager.Instance.UpdateMiddleUIModle(State);
        Manager.EnemyHandler.Initialize();
        Manager.AddGold(0);
        HasEnemySpawn = false;
    }

    public override void GameUpdate()
    {
        if (Manager != null)
        {
            Manager.PlayerHandler.PlayerMoveForward();
            MoveEnvironment(GameManager.WALK_SPEED);
            if (HasEnemySpawn == true)
            {
                if (Manager.EnemyHandler.IsEnemyInBattlePosition(GameManager.ENEMY_WALK_SPEED, Manager))
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

*/