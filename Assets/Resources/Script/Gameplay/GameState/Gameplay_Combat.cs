using RageKnight;
using RageKnight.GameState;
using System;
using System.Collections.Generic;

public class Gameplay_Combat : GameplayState_Base<GameplayState>
{
    private bool startCountDown = false;
    private Action onStateEndAction = null;
    GameUIManager Controls = null;

    public delegate void OnStateSwitch(CombatState nextState);
    public event OnStateSwitch OnStatePreSwitchEvent = null;
    private Dictionary<CombatState, Combat_Base<CombatState>> combatStates = new Dictionary<CombatState, Combat_Base<CombatState>>();
    private Combat_Base<GameplayState> comCurrentState = null;
    private CombatState comCurrentStateName;
    private CombatState compreviousStateName;

    public Gameplay_Combat(GameManager manager, RageKnight_InGame handler) : base(GameplayState.COMBAT, manager, handler)
    {
        combatStates = new Dictionary<CombatState, Combat_Base<CombatState>>();

        Combat_SpecialEVent specialEvent = new Combat_SpecialEVent(Manager, handler);
        Combat_TurnCheck turnCheck = new Combat_TurnCheck(Manager, handler);
        Combat_PassiveAction passiveAction = new Combat_PassiveAction(Manager, handler);
        Combat_StatusAction statusAction = new Combat_StatusAction(Manager, handler);
        Combat_Action action = new Combat_Action(Manager, handler);
        Combat_StatusCheck statusCheck = new Combat_StatusCheck(Manager, handler);
        Combat_Result result = new Combat_Result(Manager, handler);

        combatStates.Add(specialEvent.State, (Combat_Base<CombatState>)specialEvent);
        combatStates.Add(turnCheck.State, (Combat_Base<CombatState>)turnCheck);
        combatStates.Add(passiveAction.State, (Combat_Base<CombatState>)passiveAction);
        combatStates.Add(statusAction.State, (Combat_Base<CombatState>)statusAction);
        combatStates.Add(action.State, (Combat_Base<CombatState>)action);
        combatStates.Add(statusCheck.State, (Combat_Base<CombatState>)statusCheck);
        combatStates.Add(result.State, (Combat_Base<CombatState>)result);
    }

    private GameplayState nextState = GameplayState.ADVENTURE;

    public override void GameGoToNextState()
    {
        Handler.SwitchState(nextState);
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return (nextState == GameplayState.ADVENTURE || 
            nextState == GameplayState.RAGE ||
            nextState == GameplayState.RESULT);
    }

    public override void GameStart()
    {
        base.GameStart();
        Controls = Manager.GameUIManager;
        startCountDown = false;

        Manager.PlayerHandler.UpdateItemCount();
        Manager.PlayerHandler.PlayerResetAnimation();

        Controls.UpdateControlMode(State);
        if (handler.GetPreviousState == GameplayState.ADVENTURE)
        {
            Controls.UpdateMiddleUIModle(State);
        }
    }

    public override void GameEnd()
    {
        if (Manager?.EnemyHandler?.HasSoldiers == false)
        {
            Manager.EnemyHandler.UnSetAllEnemy();
            Manager.PlayerHandler.ResetPassiveBenifits();
        }
    }

    public override void GameUpdate()
    {
        //UnityEngine.Debug.Log(Manager.PlayerHandler.GetPlayerState);
        if (startCountDown == false && Manager != null)
        {
            CheckPlayerAction();
        }
    }

    public override void GameTimerUpdate()
    {
        if (startCountDown == true)
        {
            UnityEngine.Debug.Log("Is countdown started? " + startCountDown);
            Controls.buttonEvents = BasicMovements.None;
            GameGoToNextState();
        }
        else if (Manager != null)
        {
            CheckPlayerCooldown();
            CheckCompanionAction();
            CheckEnemyAction();
        }
    }

    private void CheckPlayerAction()
    {
        if (Controls != null)
        {
            if (Manager.PlayerHandler?.IsAlive == false)
            {
                Manager.GameOverReset();
                nextState = GameplayState.RESULT;
                startCountDown = true;
                return;
            }

            Manager.PlayerHandler?.UpdateActionGuage();
            bool canAttack = true; //Manager.PlayerHandler?.GetPlayerData.ActionGaugePoints >= 10;

            if (Controls.buttonEvents == BasicMovements.Attack && canAttack)
            {
                Manager.PlayerHandler?.PlayerAttack(Manager);
                Manager.PlayerHandler?.CheckHitTiming();
                Manager.PlayerHandler?.UseActionGauge();
            }
            else if (Controls.buttonEvents == BasicMovements.Idle)
            {
                Manager.PlayerHandler?.PlayerResetAnimation();
            }

            if (Controls.skillEvents == SkillMovements.Rage)
            {
                UnityEngine.Debug.Log("Switching to Rage Mode");
                nextState = GameplayState.RAGE;
                startCountDown = true;
            }
            else if (Controls.skillEvents == SkillMovements.Heal)
            {
                Manager.PlayerHandler?.PlayerUseItem();
            }

            Controls.buttonEvents = BasicMovements.None;
            Controls.skillEvents = SkillMovements.None;

            if (Manager.EnemyHandler.HasSoldiers == false)
            {
                UnityEngine.Debug.Log("Is enemy Alive? " + Manager.EnemyHandler.HasSoldiers);
                //TODO Improve this someday
                Manager.IncrementStage();
                nextState = Manager.IsFinalStage ? GameplayState.RESULT : GameplayState.ADVENTURE;
                startCountDown = true;
            }
        }
    }
    
    private void CheckEnemyAction()
    {
        if(Manager.EnemyHandler != null)
        {
            Manager.EnemyHandler.EnemyActionChecker(Manager);
        }
    }

    private void CheckPlayerCooldown()
    {
        if(Manager.PlayerHandler != null)
        {
            Manager.PlayerHandler.CheckRageModeDuration();
        }
    }

    private void CheckCompanionAction()
    {
        if (Manager.PlayerHandler != null)
        {
            Manager.PlayerHandler.CheckCompanionAction(Manager);
        }
    }

    private void MoveEnvironment(float speed)
    {
        if (Manager.EnvironmentHandler != null)
        {
            Manager.EnvironmentHandler.MoveEnvironment(speed);
        }
    }
}

public enum CombatState
{
    TURN_CHECK, //Who's turn it is
    SPECIAL_EVENT, //cutscene or something story related
    PASSIVE_ACTION, //Passive
    STATUS_ACTION, //buff debuff and such
    ACTION, //-> Normal Attack, Magic, Item Use, Heal, Rage, Target Select, 
    STATUS_CHECK, //Check if still alive
    SPAWN_CHECK, //spawn more enemies
    RESULT //fight result
}

public enum CombatActionStates
{
    SELECT,
    ATTACK,
    MAGIC,
    ITEM,
    HEAL,
    SKILL,
    RAGE,
    TARGET_SELECT,
    APPLY_ACTION
}

public class Combat_Base<CombatState>
{
    private CombatState state;
    private CombatState nextState;
    private GameManager manager;
    public RageKnight_InGame handler;

    public CombatState State { get { return state; } }
    public CombatState NextState { get { return nextState; } }
    public GameManager Manager { get { return manager; } }
    public RageKnight_InGame Handler { get { return handler; } }

    public Combat_Base(CombatState state, GameManager manager, RageKnight_InGame handler)
    {
        this.state = state;
        this.manager = manager;
        this.handler = handler;
    }

    public virtual bool AllowTransition()
    {
        return true;
    }

    public virtual void GoToNextState()
    {
        if (AllowTransition() == false)
        {
            UnityEngine.Debug.LogError("State = " + this.state + " Invalid Transition");
            return;
        }
    }

    public virtual void Start(){}

    public virtual void ProtoUpdate() { }

    public virtual void ProtoTimerUpdate() { }

    public virtual void End() { }

    public virtual void Destroy(){}
}

public class Combat_SpecialEVent : Combat_Base<CombatState>
{
    public Combat_SpecialEVent(GameManager manager, RageKnight_InGame handler) : base(CombatState.SPECIAL_EVENT, manager, handler)
    {

    }

    public override bool AllowTransition()
    {
        return true;
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start() { }

    public override void ProtoUpdate() { }

    public override void ProtoTimerUpdate() { }

    public override void End() { }

    public override void Destroy() { }
}

public class Combat_TurnCheck : Combat_Base<CombatState>
{
    public Combat_TurnCheck(GameManager manager, RageKnight_InGame handler) : base(CombatState.TURN_CHECK, manager, handler)
    {
    }

    public override bool AllowTransition()
    {
        return true;
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start() { }

    public override void ProtoUpdate() { }

    public override void ProtoTimerUpdate() { }

    public override void End() { }

    public override void Destroy() { }
}

public class Combat_PassiveAction : Combat_Base<CombatState>
{
    public Combat_PassiveAction(GameManager manager, RageKnight_InGame handler) : base(CombatState.PASSIVE_ACTION, manager, handler)
    {
    }

    public override bool AllowTransition()
    {
        return true;
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start() { }

    public override void ProtoUpdate() { }

    public override void ProtoTimerUpdate() { }

    public override void End() { }

    public override void Destroy() { }
}

public class Combat_StatusAction : Combat_Base<CombatState>
{
    public Combat_StatusAction(GameManager manager, RageKnight_InGame handler) : base(CombatState.STATUS_ACTION, manager, handler)
    {
    }

    public override bool AllowTransition()
    {
        return true;
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start() { }

    public override void ProtoUpdate() { }

    public override void ProtoTimerUpdate() { }

    public override void End() { }

    public override void Destroy() { }
}

public class Combat_Action : Combat_Base<CombatState>
{
    public Combat_Action(GameManager manager, RageKnight_InGame handler) : base(CombatState.ACTION, manager, handler)
    {
    }

    public override bool AllowTransition()
    {
        return true;
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start() { }

    public override void ProtoUpdate() { }

    public override void ProtoTimerUpdate() { }

    public override void End() { }

    public override void Destroy() { }
}

public class Combat_StatusCheck: Combat_Base<CombatState>
{
    public Combat_StatusCheck(GameManager manager, RageKnight_InGame handler) : base(CombatState.STATUS_CHECK, manager, handler)
    {
    }

    public override bool AllowTransition()
    {
        return true;
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start() { }

    public override void ProtoUpdate() { }

    public override void ProtoTimerUpdate() { }

    public override void End() { }

    public override void Destroy() { }
}

public class Combat_Result : Combat_Base<CombatState>
{
    public Combat_Result(GameManager manager, RageKnight_InGame handler) : base(CombatState.RESULT, manager, handler)
    {
    }

    public override bool AllowTransition()
    {
        return true;
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start() { }

    public override void ProtoUpdate() { }

    public override void ProtoTimerUpdate() { }

    public override void End() { }

    public override void Destroy() { }
}