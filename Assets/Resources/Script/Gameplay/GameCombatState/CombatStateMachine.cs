using RageKnight;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public enum CombatState
{
    SETUP, //Any setup needed
    SPECIAL_EVENT, //cutscene or something story related
    TURN_CHECK, //Who's turn it is
    PASSIVE_ACTION, //Passive
    STATUS_ACTION, //buff debuff and such
    ACTION_SELECTION, //Select action
    ACTION, //execute selecterd action -> Normal Attack, Magic, Item Use, Heal, Rage, Target Select, 
    END_TURN_CHECK, //Check if still alive and buffs that ends at this turn
    SPAWN_CHECK, //spawn more enemies
    RESULT //fight outcome
}

public enum CombatSelectionState
{
    MOVE_SELECT,
    TARGET_SELECT
}

public class CombatStateMachine
{
    public delegate void OnCombatEnd();
    public event OnCombatEnd onCombatEnd = null;

    private Dictionary<CombatState, Combat_Base<CombatState>> combatStates = new Dictionary<CombatState, Combat_Base<CombatState>>();
    private Combat_Base<CombatState> currentState = null;
    private bool isInit = false;

    private GameManager gameManager;
    public GameManager GetGameManager
    {
        get
        {
            return gameManager;
        }
    }

    public CombatStateMachine(GameManager manager, Gameplay_Combat handler)
    {
        gameManager = manager;

        combatStates = new Dictionary<CombatState, Combat_Base<CombatState>>();

        Combat_Setup setup = new Combat_Setup(this);
        Combat_SpecialEvent specialEvent = new Combat_SpecialEvent(this);
        Combat_TurnCheck turnCheck = new Combat_TurnCheck(this);
        Combat_PassiveAction passiveAction = new Combat_PassiveAction(this);
        Combat_StatusAction statusAction = new Combat_StatusAction(this);
        Combat_ActionSelection actionSelection = new Combat_ActionSelection(this);
        Combat_Action action = new Combat_Action(this);
        Combat_EndTurnCheck endturnCheck = new Combat_EndTurnCheck(this);
        Combat_SpawnCheck spawnCheck = new Combat_SpawnCheck(this);
        Combat_Result result = new Combat_Result(this);

        combatStates.Add(setup.State, setup);
        combatStates.Add(specialEvent.State, specialEvent);
        combatStates.Add(turnCheck.State, turnCheck);
        combatStates.Add(passiveAction.State, passiveAction);
        combatStates.Add(statusAction.State, statusAction);
        combatStates.Add(actionSelection.State, actionSelection);
        combatStates.Add(action.State, action);
        combatStates.Add(endturnCheck.State, endturnCheck);
        combatStates.Add(spawnCheck.State, spawnCheck);
        combatStates.Add(result.State, result);

        onCombatEnd = new OnCombatEnd(handler.NextState);

        currentState = combatStates[GetNextState()];
        isInit = true;
    }

    public void Start()
    {
        //We start with an empty state. Doing this will start the SETUP State.
        currentState.Start();
    }

    public void Update()
    {
        if (currentState != null && isInit)
            currentState.ProtoUpdate();
    }

    public void TimerUpdate()
    {
        if (currentState != null && isInit)
            currentState.ProtoTimerUpdate();
    }

    public void End()
    {
    }

    public void Destroy()
    {
        if (combatStates != null)
        {
            foreach (CombatState key in combatStates.Keys)
            {
                Debug.Log("Destroyed Combat State" + key);
                combatStates[key].Destroy();
            }
            combatStates.Clear();
            combatStates = null;
        }
    }

    private CombatState GetNextState()
    {
        if (currentState == null)
            return CombatState.SETUP;

        else if (CombatState.SETUP == currentState.State)
            return CombatState.SPECIAL_EVENT;

        else if (CombatState.SPECIAL_EVENT == currentState.State)
            return CombatState.TURN_CHECK;

        else if (CombatState.TURN_CHECK == currentState.State)
            return CombatState.PASSIVE_ACTION;

        else if (CombatState.PASSIVE_ACTION == currentState.State)
            return CombatState.STATUS_ACTION;

        else if (CombatState.STATUS_ACTION == currentState.State)
            return CombatState.ACTION_SELECTION;

        else if (CombatState.ACTION_SELECTION == currentState.State)
            return CombatState.ACTION;

        else if (CombatState.ACTION == currentState.State)
            return CombatState.END_TURN_CHECK;

        else if (CombatState.END_TURN_CHECK == currentState.State)
            return CombatFinish ? CombatState.RESULT : CombatState.SPAWN_CHECK;

        else //CombatState.SPAWN_CHECK
            return CombatState.SETUP;
    }

    public bool CombatFinish = false; //Temp
    public void NextState()
    {
        GameManager.Instance.ExecuteRoutine(NextStateRoutine());
    }

    private System.Collections.IEnumerator NextStateRoutine()
    {
        yield return new WaitForEndOfFrame();
        currentState.End();
        Debug.Log("CURRENT STATE " + currentState.State.ToString());
        if (currentState?.State != CombatState.RESULT)
        {
            var nextState = GetNextState();
            Debug.Log("NEXT STATE " + nextState.ToString());
            currentState = combatStates[GetNextState()];
            yield return new WaitForSeconds(0.1f);
            currentState.Start();
            GameUIManager.Instance.DebugUpdateGamePlayState(currentState.State.ToString());
        }
        else if (currentState?.State == CombatState.RESULT)
        {
            End();
            GameManager.Instance.StateMachine.SwitchState(GameplayState.RESULT);
        }
        else
        {
            Debug.LogError("Something went wrong");
        }
    }
}




///////////////////////////////////




public class Combat_Base<CombatState>
{
    private CombatState state;
    private CombatState nextState;
    private CombatStateMachine csMachine;
    
    public CombatState State { get { return state; } }
    public CombatState NextState { get { return nextState; } }
    public CombatStateMachine CSMachine { get { return csMachine; } }

    public Combat_Base(CombatState state, CombatStateMachine machine)
    {
        this.state = state;
        this.csMachine = machine;
    }

    public virtual void GoToNextState()
    {
        CSMachine.NextState();
    }

    public virtual void Start() { }

    public virtual void ProtoUpdate() { }

    public virtual void ProtoTimerUpdate() {}

    public virtual void End() { }

    public virtual void Destroy() { }
}

//Setup combat UI //Show initial Monsters
public class Combat_Setup : Combat_Base<CombatState>
{
    public Combat_Setup(CombatStateMachine machine) : base(CombatState.SETUP, machine)
    {
        bool isPlayerTurn = true;
        CSMachine.GetGameManager.PlayerHandler.SetTurnOrder(isPlayerTurn);
        CSMachine.GetGameManager.EnemyHandler.SetTurnOrder(isPlayerTurn);
        CSMachine.GetGameManager.PlayerHandler.ResetTurns();
        Debug.Log("Set Turn");
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Special event like monster additional stuff
public class Combat_SpecialEvent : Combat_Base<CombatState>
{
    public Combat_SpecialEvent(CombatStateMachine machine) : base(CombatState.SPECIAL_EVENT, machine)
    {

    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Check who tunr it is also
public class Combat_TurnCheck : Combat_Base<CombatState>
{
    public Combat_TurnCheck(CombatStateMachine machine) : base(CombatState.TURN_CHECK, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        string currentAtTurnCombatId = GameManager.Instance.GetCurrentAtTurnUnitCombatId;
        GameUIManager.Instance.HealthbarHandler.UpdateActiveStatus(currentAtTurnCombatId);
        GameUIManager.Instance.UpdateChracterInAction(currentAtTurnCombatId);
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Activate passive action
public class Combat_PassiveAction : Combat_Base<CombatState>
{
    public Combat_PassiveAction(CombatStateMachine machine) : base(CombatState.PASSIVE_ACTION, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Activate good and bad status
public class Combat_StatusAction : Combat_Base<CombatState>
{
    public Combat_StatusAction(CombatStateMachine machine) : base(CombatState.STATUS_ACTION, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Wait for player to decide action
public class Combat_ActionSelection : Combat_Base<CombatState>
{
    private bool actionSelected = false;
    private bool isInit = false;
    private bool targetSelected = false;
    private bool isPlayerTurn;
    public Combat_ActionSelection(CombatStateMachine machine) : base(CombatState.ACTION_SELECTION, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        isPlayerTurn = GameManager.Instance.GetIsPlayerTurn;
        actionSelected = false;
        isInit = true;
        if (isPlayerTurn)
        {
            Debug.Log("PLAYER TURN");
            GameUIManager.Instance.ResetSelections();
            GameUIManager.Instance.AllowPlayerCommands();
        }
        else
            Debug.Log("ENEMY TURN");

        base.Start();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
        if (actionSelected == true || isInit == false)
            return;

        if (isPlayerTurn &&
            GameTargetingManager.Instance.GetIsTargetSelectionDone &&
            GameUIManager.Instance.GetButtonEvent != CombatAction.NONE)
        {
            actionSelected = true;
            Debug.Log("ATTACK");
            GoToNextState();
        }
        else if(isPlayerTurn == false)
        {
            GameTargetingManager.Instance.SetRandomPlayerUnitTarget();
            GameManager.Instance.EnemyHandler.EnemyActionChecker();
            actionSelected = true;
            Debug.Log("ENEMY ATTACK");
            GoToNextState();
        }
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        isInit = false;
        GameUIManager.Instance.PreventPlayerCommands();
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Attack, skill, item, run away
public enum CombatActionExecutionState
{
    NONE,
    RUN_TOWARDS,
    RUN_BACK,
    DONE
}
public class Combat_Action : Combat_Base<CombatState>
{
    private Transform targetPosition;
    private float speed = 4f;
    private bool isPlayerUnitsTurn;
    CombatActionExecutionState caExecutionState = CombatActionExecutionState.NONE;
    public Combat_Action(CombatStateMachine machine) : base(CombatState.ACTION, machine)
    {
    }
    private float GetSpeed
    {
        get
        {
            return (float)Math.Round(Time.deltaTime * speed, 2);
        }
    }
    public override void GoToNextState()
    {
        Debug.Log("GoToNextState");
        caExecutionState = CombatActionExecutionState.DONE;
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        isPlayerUnitsTurn = GameManager.Instance.GetIsPlayerTurn;
        List<string> targets = GameTargetingManager.Instance.GetTargets;
        if(isPlayerUnitsTurn)
            targetPosition = GameManager.Instance.EnemyHandler.GetUnitTransform(targets.FirstOrDefault());
        else
            targetPosition = GameManager.Instance.PlayerHandler.GetUnitTransform(targets.FirstOrDefault());
        //check if Melee
        caExecutionState = CombatActionExecutionState.RUN_TOWARDS;
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
        if (caExecutionState == CombatActionExecutionState.NONE || caExecutionState == CombatActionExecutionState.DONE)
            return;

        if (caExecutionState == CombatActionExecutionState.RUN_TOWARDS)
        {
            RunTowards();
        }
        else if (caExecutionState == CombatActionExecutionState.RUN_BACK)
        {
            RunBack();
        }
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        Debug.Log("End State");
        GameUIManager.Instance.ResetSelections();
        GameTargetingManager.Instance.ResetSelections();
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    private void RunTowards()
    {
        Action callback = () => {
            caExecutionState = CombatActionExecutionState.NONE;
            ExecuteAction();
        };

        if (isPlayerUnitsTurn)
            GameManager.Instance.PlayerHandler.RunTowards(targetPosition, GetSpeed, callback);
        else
            GameManager.Instance.EnemyHandler.RunTowards(targetPosition, GetSpeed, callback);
    }
    private void ExecuteAction()
    {
        if (isPlayerUnitsTurn && GameUIManager.Instance.GetButtonEvent == CombatAction.ATTACK)
        {
            List<string> targets = GameTargetingManager.Instance.GetTargets;
            CSMachine.GetGameManager.PlayerHandler.PlayerAttack(() => OnAttackEnd());
            CSMachine.GetGameManager.EnemyHandler.DamagedEnemy(10, targets);
        }
        else
        {
            List<string> targets = GameTargetingManager.Instance.GetTargets;
            CSMachine.GetGameManager.EnemyHandler.ExecuteAction(() => OnAttackEnd());
            CSMachine.GetGameManager.PlayerHandler.DamagedPlayer(10, targets);
        }
    }
    private void OnAttackEnd()
    {
        caExecutionState = CombatActionExecutionState.RUN_BACK;
    }
    private void RunBack()
    {
        if (isPlayerUnitsTurn)
            GameManager.Instance.PlayerHandler.RunBackToSpot(GetSpeed, () => GoToNextState());
        else
            GameManager.Instance.EnemyHandler.RunBackToSpot(GetSpeed, () => GoToNextState());
    }
}

//Check if combat status if both side are still alive and end turn stuff
public class Combat_EndTurnCheck : Combat_Base<CombatState>
{
    public Combat_EndTurnCheck(CombatStateMachine machine) : base(CombatState.END_TURN_CHECK, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        CSMachine.CombatFinish = CSMachine.GetGameManager.EnemyHandler.IsAlive == false;
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        if (GameManager.Instance.GetIsPlayerTurn)
        {
            Debug.Log("PLAYER TURN END");
            CSMachine.GetGameManager.PlayerHandler.TurnEnd();
            CSMachine.GetGameManager.PlayerHandler.UpdateTurns();
            if (GameManager.Instance.GetIsPlayerTurn == false)
            {
                Debug.Log("ALL PLAYER TURN END");
                CSMachine.GetGameManager.EnemyHandler.ResetTurns();
            }
        }
        else 
        {
            Debug.Log("ENEMY TURN END");
            CSMachine.GetGameManager.EnemyHandler.TurnEnd();
            CSMachine.GetGameManager.EnemyHandler.UpdateTurns();
            if (GameManager.Instance.GetIsEnemyTurn == false)
            {
                Debug.Log("ALL ENEMY TURN END");
                CSMachine.GetGameManager.PlayerHandler.ResetTurns();
            }
        }
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Check if there are still monsters
public class Combat_SpawnCheck : Combat_Base<CombatState>
{
    public Combat_SpawnCheck(CombatStateMachine machine) : base(CombatState.SPAWN_CHECK, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        CSMachine.CombatFinish = GameManager.Instance.EnemyHandler.IsAlive == false 
            || GameManager.Instance.PlayerHandler.IsAlive == false;
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

//Result
public class Combat_Result : Combat_Base<CombatState>
{
    public Combat_Result(CombatStateMachine machine) : base(CombatState.RESULT, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
        //Calculate result
        GoToNextState();
    }

    public override void ProtoUpdate()
    {
        base.ProtoUpdate();
    }

    public override void ProtoTimerUpdate()
    {
        base.ProtoTimerUpdate();
    }

    public override void End()
    {
        base.End();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
