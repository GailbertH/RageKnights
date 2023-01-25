using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.GameState
{
    public class RageKnight_Loading : RageKnightState_Base<RageKnightState>
    {
        private Coroutine cdNextState;

        public RageKnight_Loading(GameManager manager) : base(RageKnightState.LOADING, manager)
        {
        }

        public override void GoToNextState()
        {
            Manager.StateMachine.SwitchState(RageKnightState.INGAME);
        }

        public override bool AllowTransition(RageKnightState nextState)
        {
            return (nextState == RageKnightState.INGAME);
        }

        public override void Start()
        {
            LoadAccountData();
            cdNextState = Manager.StartCoroutine(DelayedStateSwitch(2f));
        }

        public override void End()
        {
            if (cdNextState != null && Manager != null)
                Manager.StopCoroutine(cdNextState);

            LoadingManager.Instance?.OnLoadBarFull();
            cdNextState = null;
        }

        public override void Destroy()
        {
            End();
            base.Destroy();
        }


        private IEnumerator DelayedStateSwitch(float delay)
        {
            yield return new WaitForSeconds(delay);
            //SoundManager.Instance.PlayBGM ();
            //Manager.LoadingScreenPlay ();
            GoToNextState();
        }

        //TODO use proper data
        private void LoadAccountData()
        {
            PlayerUnitModel playerData = null;
#if UNITY_EDITOR
            if (Manager.isTestMode == true)
            {
                playerData = new PlayerUnitModel
                {
                    AttackPower = 2,
                    DefensePower = 2,
                    VitalityPower = 2,
                    RageIncrement = 1,
                    HealthPoints = 100,
                    ManaPoints = 100,
                    RagePoints = 0
                };
            }
#endif
            if (playerData == null)
            {
                if (AccountManager.Instance.AccountData.HasCurrentActiveGame == false)
                {
                    AccountManager.Instance.ResetPlayerStatus();
                }
                AccountManager.Instance.SetActiveGame(true, true);
                playerData = AccountManager.Instance.AccountData.CurrentCharacterData;
            }
            Manager.AccountDataInit(playerData);
        }
    }
}