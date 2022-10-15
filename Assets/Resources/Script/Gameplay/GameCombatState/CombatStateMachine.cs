using RageKnight;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum CombatState
{
    SETUP, //Any setup needed
    SPECIAL_EVENT, //cutscene or something story related
    TURN_CHECK, //Who's turn it is
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

public class CombatStateMachine
{
    public delegate void OnCombatEnd();
    public event OnCombatEnd onCombatEnd = null;

    private Dictionary<CombatState, Combat_Base<CombatState>> combatStates = new Dictionary<CombatState, Combat_Base<CombatState>>();
    private Combat_Base<CombatState> currentState = null;

    public CombatStateMachine(GameManager manager, Gameplay_Combat handler)
    {
        combatStates = new Dictionary<CombatState, Combat_Base<CombatState>>();

        Combat_Setup setup = new Combat_Setup(this);
        Combat_SpecialEvent specialEvent = new Combat_SpecialEvent(this);
        Combat_TurnCheck turnCheck = new Combat_TurnCheck(this);
        Combat_PassiveAction passiveAction = new Combat_PassiveAction(this);
        Combat_StatusAction statusAction = new Combat_StatusAction(this);
        Combat_Action action = new Combat_Action(this);
        Combat_StatusCheck statusCheck = new Combat_StatusCheck(this);
        Combat_SpawnCheck spawnCheck = new Combat_SpawnCheck(this);
        Combat_Result result = new Combat_Result(this);

        combatStates.Add(setup.State, setup);
        combatStates.Add(specialEvent.State, specialEvent);
        combatStates.Add(turnCheck.State, turnCheck);
        combatStates.Add(passiveAction.State, passiveAction);
        combatStates.Add(statusAction.State, statusAction);
        combatStates.Add(action.State, action);
        combatStates.Add(statusCheck.State, statusCheck);
        combatStates.Add(spawnCheck.State, spawnCheck);
        combatStates.Add(result.State, result);

        onCombatEnd = new OnCombatEnd(handler.NextState);

        currentState = combatStates[GetNextState()];
    }

    public void Start()
    {
        //We start with an empty state. Doing this will start the SETUP State.
        currentState.Start();
    }

    public void Update()
    {
        if (currentState != null)
            currentState.ProtoUpdate();
    }

    public void TimerUpdate()
    {
        if (currentState != null)
            currentState.ProtoTimerUpdate();
    }

    public void Destroy()
    {
        if (combatStates != null)
        {
            foreach (CombatState key in combatStates.Keys)
            {
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
            return CombatState.ACTION;

        else if (CombatState.ACTION == currentState.State)
            return CombatState.STATUS_CHECK;

        else if (CombatState.STATUS_CHECK == currentState.State)
            return CombatFinish ? CombatState.RESULT : CombatState.SPAWN_CHECK;

        else //CombatState.SPAWN_CHECK
            return CombatState.SETUP;
    }

    public bool CombatFinish = true;
    public void NextState()
    {
        currentState?.End();
        Debug.Log("CURRENT STATE " + currentState.State.ToString());
        if (currentState.State == CombatState.RESULT)
        {
            End();
            return;
        }
        var nextState = GetNextState();
        Debug.Log("NEXT STATE " + nextState.ToString());
        currentState = combatStates[GetNextState()];
        currentState.Start();
    }

    public void End()
    {
        //onCombatEnd.Invoke();
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

    public virtual void Start() { GoToNextState(); }

    public virtual void ProtoUpdate() { }

    public virtual void ProtoTimerUpdate() { }

    public virtual void End() { }

    public virtual void Destroy() { }
}

public class Combat_Setup : Combat_Base<CombatState>
{
    public Combat_Setup(CombatStateMachine machine) : base(CombatState.SETUP, machine)
    {

    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
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

public class Combat_Action : Combat_Base<CombatState>
{
    public Combat_Action(CombatStateMachine machine) : base(CombatState.ACTION, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
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

public class Combat_StatusCheck : Combat_Base<CombatState>
{
    public Combat_StatusCheck(CombatStateMachine machine) : base(CombatState.STATUS_CHECK, machine)
    {
    }

    public override void GoToNextState()
    {
        base.GoToNextState();
    }

    public override void Start()
    {
        base.Start();
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
