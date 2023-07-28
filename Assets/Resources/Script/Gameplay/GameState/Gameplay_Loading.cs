using RageKnight;
using RageKnight.Database;
using System;
using System.Collections.Generic;

public class Gameplay_Loading : GameplayState_Base<GameplayState>
{
    private bool allTaskDone = false;
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

        LoadPlayerUnitData();
        LoadEnemyUnitData();

        GameUIManager.Instance.UpdateControlMode(State);
        GameUIManager.Instance.UpdateMiddleUIModle(State);

        allTaskDone = true; //Better as a callback
    }

    public override void GameUpdate()
    {
        base.GameUpdate();
        if (allTaskDone == true)
        {
            GameGoToNextState();
        }
    }

    public override void GameTimerUpdate()
    {
        base.GameTimerUpdate();
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
    private void LoadPlayerUnitData()
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

    private void LoadEnemyUnitData()
    {
        List<string> strings = new List<string> { "0", "0", "0" };
        List<UnitDataModel> dataModels = new List<UnitDataModel>();
        dataModels = DatabaseManager.Instance.GetEnemyUnits(strings);

        List<UnitModel> enemyDataList = new List<UnitModel>();
        foreach (UnitDataModel dataModel in dataModels)
        {
            UnitModel enemyData = new UnitModel
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

                healthPoints = dataModel.healthPoints,
                manaPoints = 100,

            };
            enemyDataList.Add(enemyData);
        }
        GameManager.Instance.EnemyHandler.EnemyInitialize(enemyDataList, strings.Count);
    }

    #endregion
}