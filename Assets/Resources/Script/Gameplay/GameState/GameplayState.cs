using RageKnight;
using RageKnight.GameState;
using UnityEngine;

public enum GameplayState
{
    LOADING = 0,
    COMBAT = 1,
    RESULT = 2,
    EXIT = 3
}

public class GameplayState_Base<GameplayState>
{
    private GameplayState state;
    protected GameManager manager;
    public GameplayStateMachine handler;

    public GameplayState State { get { return state; } }
    public GameManager Manager { get { return manager; } }
    public GameplayStateMachine Handler { get { return handler; } }

    public GameplayState_Base(GameplayState state)
    {
        manager = GameManager.Instance;
        handler = GameManager.Instance.StateMachine;
        this.state = state;
    }

    public virtual bool GameAllowTransition(GameplayState nextState)
    {
        return true;
    }

    public virtual void GameGoToNextState() { }

    public virtual void GameStart()
    {
        Debug.Log(this.state.ToString());
    }

    public virtual void GameUpdate() { }

    public virtual void GameTimerUpdate() { }

    public virtual void GameEnd() { }

    public virtual void GameDestroy()
    {
        state = default;
        handler = null;
        manager = null;
    }
}
