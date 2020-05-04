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

        private void LoadAccountData()
        {
            PlayerModel playerData = null;
#if UNITY_EDITOR
            if (Manager.isTestMode == true)
            {
                playerData = new PlayerModel
                {
                    AttackPower = 2,
                    DefensePower = 2,
                    HealthPower = 2,
                    RagePower = 2,
                    RageIncrement = 0.5f,
                    ActionGaugeIncrement = 1f,
                    HealthPoints = 10f,
                    ActionGaugePoints = 10f,
                    RagePoints = 0,
                    ItemCount = 5,
                    WeapomStatBonus = 0,
                    HealthStatBonus = 0,
                    ArmorStatBonus = 0,
                    BaseHealthPoints = 10f,
                    MaxRagePoints = 100f,
                    MaxActionGaugePoints = 100f,
                    MaxItemCount = 10,
                    AttackRageMultiplier = 2
                };
            }
#endif
            if (playerData == null)
            {
                playerData = AccountManager.Instance.AccountData.CurrentCharacterData;
            }
            long gold = AccountManager.Instance.AccountData.Gold;

            Manager.AccountDataInit(playerData, gold);
        }
    }
}