using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.GameState
{
	public class RageKnight_Loading : RageKnightState_Base<RageKnightState>
	{
		private Coroutine cdNextState;

		public RageKnight_Loading (GameManager manager) : base (RageKnightState.LOADING, manager)
		{
		}

		public override void GoToNextState()
		{
			Manager.StateMachine.SwitchState (RageKnightState.INGAME);
		}

		public override bool AllowTransition (RageKnightState nextState)
		{
			return (nextState == RageKnightState.INGAME);
		}

		public override void Start ()
		{
			//Loading hahahaha
			cdNextState = Manager.StartCoroutine (DelayedStateSwitch (2f));
		}

		public override void End () 
		{
			if (cdNextState != null && Manager != null)
				Manager.StopCoroutine (cdNextState);

			cdNextState = null;
		}

		public override void Destroy ()
		{
			End ();
			base.Destroy ();
		}


		private IEnumerator DelayedStateSwitch (float delay)
		{
			yield return new WaitForSeconds(delay);
			//SoundManager.Instance.PlayBGM ();
			//Manager.LoadingScreenPlay ();
			GoToNextState ();
		}
	}
}