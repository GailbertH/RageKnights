using RageKnight;
using System;
using System.Collections.Generic;

public class Gameplay_Loading : GameplayState_Base<GameplayState>
{
    private bool HasEnemySpawn = false;
    public Gameplay_Loading() : base(GameplayState.LOADING)
    {
        //Load units data
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
        GameManager.Instance.EnemyHandler.Initialize();
        LoadAccountData();
        HasEnemySpawn = false;
    }

    public override void GameUpdate()
    {
        base.GameUpdate();
        if (HasEnemySpawn == true)
        {
            if (GameManager.Instance.EnemyHandler.IsEnemyInBattlePosition(GameManager.ENEMY_WALK_SPEED, Manager))
            {
                GameGoToNextState();
            }
        }
    }

    public override void GameTimerUpdate()
    {
        base.GameTimerUpdate();
        CheckIfEnemySpawned();
    }
    public override void GameGoToNextState()
    {
        Manager.StateMachine.SwitchState(GameplayState.COMBAT);
    }

    public override void GameEnd()
    {
        base.GameEnd();
        LoadingManager.Instance?.OnLoadBarFull();
    }

    #region private methods
    //TODO use proper data
    private void LoadAccountData()
    {
        string combatId = Guid.NewGuid().ToString();
        List<PlayerUnitModel> playerDataList = new List<PlayerUnitModel>();
        string[] unitNames = { "Lancelot", "Vira", "Albert" };
        for (int i = 0; i < unitNames.Length; i++)
        {
            PlayerUnitModel playerData = new PlayerUnitModel
            {
                name = unitNames[i],
                unitCombatID = combatId,
                healthPoints = 100,
                manaPoints = 100,
                ragePoints = 0,
                rageIncrement = 1,
                attackPower = 2,
                defensePower = 2,
                vitalityPower = 2
            };

            playerDataList.Add(playerData);
        }
        Manager.AccountDataInit(playerDataList);
    }


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
    #endregion
}