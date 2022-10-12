
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.GameState
{
    public class RageKnightStateMachine
    {
        public delegate void OnStateSwitch(RageKnightState nextState);
        public event OnStateSwitch OnStatePreSwitchEvent = null;

        private Dictionary<RageKnightState, RageKnightState_Base<RageKnightState>> states = new Dictionary<RageKnightState, RageKnightState_Base<RageKnightState>>();
        private RageKnightState_Base<RageKnightState> currentState = null;
        private List<RageKnightState> prevGameState;

        public RageKnightStateMachine(GameManager manager)
        {
            states = new Dictionary<RageKnightState, RageKnightState_Base<RageKnightState>>();

            RageKnight_Loading loading = new RageKnight_Loading(manager);
            RageKnight_InGame inGame = new RageKnight_InGame(manager);
            RageKnight_Exit exit = new RageKnight_Exit(manager);

            states.Add(loading.State, (RageKnightState_Base<RageKnightState>)loading);
            states.Add(inGame.State, (RageKnightState_Base<RageKnightState>)inGame);
            states.Add(exit.State, (RageKnightState_Base<RageKnightState>)exit);

            prevGameState = new List<RageKnightState>();

            SwitchState(RageKnightState.LOADING);
        }

        public void Update()
        {
            if (currentState != null)
                currentState.Update();
        }

        public void TimerUpdate()
        {
            if (currentState != null)
                currentState.TimerUpdate();
        }

        public void Destroy()
        {
            if (states != null)
            {
                foreach (RageKnightState key in states.Keys)
                {
                    states[key].Destroy();
                }
                states.Clear();
                states = null;
            }
        }

        public RageKnightState_Base<RageKnightState> GetCurrentState
        {
            get { return currentState; }
        }

        public string GetPreviousStateList()
        {
            string prevStates = "PREVIOUS STATES: ";

#if UNITY_EDITOR
            if (prevGameState != null)
            {
                for (int i = prevGameState.Count - 1; i >= 0; i--)
                {
                    prevStates += "\n-> " + prevGameState[i].ToString();
                }
            }
#endif

            return prevStates;
        }

        public void Exit()
        {
            SwitchState(RageKnightState.EXIT);
        }

		public bool SwitchState (RageKnightState newState)
		{
			bool switchSuccess = false;
            Debug.Log(newState);
			if (states != null && states.ContainsKey (newState))
			{
				if (currentState == null)
				{
					currentState = states [newState];
					currentState.Start ();
					switchSuccess = true;
				}
				else if (currentState.AllowTransition (newState))
				{
					currentState.End ();
					currentState = states [newState];
					currentState.Start ();
					switchSuccess = true;
				}
				else
				{
					Debug.Log (string.Format ("{0} does not allow transition to {1}", currentState.State, newState));
				}
			}

			if (switchSuccess)
			{
				// Updating state history
				#if UNITY_EDITOR
				if(prevGameState != null)
				{
					prevGameState.Add(newState);
					if(prevGameState.Count > 20)
					{
						prevGameState.RemoveAt(0);
					}
				}
				#endif

				if (this.OnStatePreSwitchEvent != null)
				{
					this.OnStatePreSwitchEvent (newState);
				}
			}
			else
			{
				Debug.Log ("States dictionary not ready for switching to " + newState);
			}

			return switchSuccess;
		}
	}
}