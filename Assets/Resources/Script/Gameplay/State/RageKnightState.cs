using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.GameState
{
    public enum RageKnightState
    {
        LOADING = 0,
        INGAME = 1,
        EXIT = 2
    }

    public class RageKnightState_Base<RageKnightState>
    {
        private RageKnightState state;
        private GameManager manager;

        public RageKnightState State { get { return state; } }
        public GameManager Manager { get { return manager; } }

        public RageKnightState_Base(RageKnightState state, GameManager manager)
        {
            this.state = state;
            this.manager = manager;
        }

        public virtual bool AllowTransition (RageKnightState nextState)
        {
            return true;
        }

        public virtual void GoToNextState() {}

        public virtual void Start () 
        {
            Debug.Log (this.state.ToString ());
        }

        public virtual void Update () {}

        public virtual void TimerUpdate() {}

        public virtual void End () {}

        public virtual void Destroy () 
        {
            End ();
            manager = null;
        }
    }
}
