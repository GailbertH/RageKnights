using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.GameState
{
	public class RageKnight_InGame : RageKnightState_Base<RageKnightState>
	{
        public delegate void OnStateSwitch(GameplayState nextState);
        public event OnStateSwitch OnStatePreSwitchEvent = null;

        private Dictionary<GameplayState, GameplayState_Base<GameplayState>> states = new Dictionary<GameplayState, GameplayState_Base<GameplayState>>();
        private GameplayState_Base<GameplayState> currentState = null;
        private GameplayState currentStateName;
        private GameplayState previousStateName;

        public GameplayState GetCurrentState
        {
            get
            {
                return currentStateName;
            }
        }

        public GameplayState GetPreviousState
        {
            get
            {
                return previousStateName;
            }
        }

        public RageKnight_InGame (GameManager manager) : base (RageKnightState.INGAME, manager)
		{
            states = new Dictionary<GameplayState, GameplayState_Base<GameplayState>>();

            Gameplay_Combat combat = new Gameplay_Combat();
            Gameplay_Result result = new Gameplay_Result();
            Gameplay_Exit exit = new Gameplay_Exit();

            states.Add(combat.State, (GameplayState_Base<GameplayState>)combat);
            states.Add(result.State, (GameplayState_Base<GameplayState>)result);
            states.Add(exit.State, (GameplayState_Base<GameplayState>)exit);

            currentStateName = GameplayState.COMBAT;
            previousStateName = GameplayState.EXIT;
        }

		public override void GoToNextState()
		{
			//Manager.StateMachine.SwitchState (RageKnightState.EXIT);
		}

		public override bool AllowTransition (RageKnightState nextState)
		{
			return (nextState == RageKnightState.EXIT);
		}
		public override void Start ()
        {
            if(currentState != null)
                currentState.GameStart();
        }

        public override void Update ()
        {
            if (currentState != null)
                currentState.GameUpdate();
        }

        public override void TimerUpdate() 
        {
            if(currentState != null)
                currentState.GameTimerUpdate();
        }

		public override void End ()
        {
            if (states != null)
            {
                foreach (var state in states)
                {
                    states[state.Key].GameDestroy();
                }
            }
            states = null;
            currentState = null;
            OnStatePreSwitchEvent = null;
        }


        public bool SwitchState(GameplayState newState)
        {
            bool switchSuccess = false;
            if (states != null && states.ContainsKey(newState))
            {
                if (currentState == null)
                {
                    currentStateName = newState;
                    currentState = states[newState];
                    currentState.GameStart();
                    switchSuccess = true;
                }
                else if (currentState.GameAllowTransition(newState))
                {
                    previousStateName = currentState.State;
                    currentStateName = newState;
                    currentState.GameEnd();
                    currentState = states[newState];
                    currentState.GameStart();
                    switchSuccess = true;
                }
                else
                {
                    Debug.Log(string.Format("{0} does not allow transition to {1}", currentState.State, newState));
                }
            }

            if (switchSuccess)
            {
                this.OnStatePreSwitchEvent?.Invoke(newState);
            }
            else
            {
                Debug.Log("States dictionary not ready for switching to " + newState);
            }

            return switchSuccess;
        }
    }
}