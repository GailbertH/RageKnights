using RageKnight;
using RageKnight.Database;
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
        LoadUnitData();
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
    private void LoadUnitData()
    {
        List<string> strings = new List<string> {"0", "1", "2" };
        List<UnitDataModel> dataModels = new List<UnitDataModel>();
        dataModels = DatabaseManager.Instance.GetPlayerUnits(strings);

        List<UnitModel> playerDataList = new List<UnitModel>();
        foreach (UnitDataModel dataModel in dataModels)
        {
            UnitModel playerData = new UnitModel
            {
                name = dataModel.name,
                unitID = dataModel.id,
                unitCombatID = Guid.NewGuid().ToString(),
                icon = dataModel.icon,
                splashArt = dataModel.splashArt,
                unitPrefab = dataModel.unitPrefab,

                attackPower = dataModel.attackPower,
                defensePower = dataModel.defensePower,
                maxHealthPoints = dataModel.healthPoints,

                healthPoints = 100,
                manaPoints = 100,

            };
            playerDataList.Add(playerData);
        }
        GameManager.Instance.PlayerUnitsInit(playerDataList);
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