using System.Collections.Generic;
using UnityEngine;

public class GameplayStateMachine
{
    private Dictionary<GameplayState, GameplayState_Base<GameplayState>> states = null;
    private GameplayState_Base<GameplayState> currentState = null;

    public GameplayStateMachine()
    {
        states = new Dictionary<GameplayState, GameplayState_Base<GameplayState>>();

        Gameplay_Loading loading = new Gameplay_Loading();
        Gameplay_Combat combat = new Gameplay_Combat();
        Gameplay_Result result = new Gameplay_Result();
        Gameplay_Exit exit = new Gameplay_Exit();

        states.Add(loading.State, (GameplayState_Base<GameplayState>)loading);
        states.Add(combat.State, (GameplayState_Base<GameplayState>)combat);
        states.Add(result.State, (GameplayState_Base<GameplayState>)result);
        states.Add(exit.State, (GameplayState_Base<GameplayState>)exit);

        SwitchState(GameplayState.LOADING);
    }

    public void Start()
    {
        currentState.GameStart();
    }
    public void Update()
    {
        if (currentState != null)
            currentState.GameUpdate();
    }

    public void TimerUpdate()
    {
        if (currentState != null)
            currentState.GameTimerUpdate();
    }
    public void End()
    {
        currentState.GameEnd();
        if (currentState.State == GameplayState.EXIT)
        {
            Exit();
        }
    }
    public void Destroy()
    {
        if (states != null)
        {
            foreach (GameplayState key in states.Keys)
            {
                Debug.Log("Destroyed Gameplay State" + key);
                states[key].GameDestroy();
            }
            states.Clear();
            states = null;
        }
    }
    public void Exit()
    {
        Destroy();
    }

    public void SwitchState(GameplayState newState)
    {
        Debug.Log("GAME STATE MACHINE " + newState);
        if (states != null && states.ContainsKey(newState))
        {
            if (currentState == null)
            {
                currentState = states[newState];
                Start();
            }
            else if (currentState.GameAllowTransition(newState))
            {
                End();
                currentState = states[newState];
                Start();
            }
            else
            {
                Debug.Log(string.Format("{0} does not allow transition to {1}", currentState.State, newState));
            }
        }
    }

    public void FinishState()
    {
        if (currentState.State == GameplayState.EXIT)
        {
            End();
        }
    }
}
