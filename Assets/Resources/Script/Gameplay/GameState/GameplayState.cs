using RageKnight;
using RageKnight.GameState;
using UnityEngine;

public enum GameplayState
{
    ADVENTURE = 0,
    COMBAT = 1,
    RESULT = 2,
    EXIT = 3
}

public class GameplayState_Base<GameplayState>
{
    private GameplayState state;
    private GameManager manager;
    public RageKnight_InGame handler;

    public GameplayState State { get { return state; } }
    public GameManager Manager { get { return manager; } }
    public RageKnight_InGame Handler { get { return handler; } }

    public GameplayState_Base(GameplayState state, GameManager manager, RageKnight_InGame handler)
    {
        this.state = state;
        this.manager = manager;
        this.handler = handler;
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
