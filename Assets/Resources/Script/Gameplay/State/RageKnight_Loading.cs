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
            PlayerModel playerData = null;
#if UNITY_EDITOR
            if (Manager.isTestMode == true)
            {
                var defaultItemCount = 5;
                playerData = new PlayerModel
                {
                    AttackPower = 2,
                    DefensePower = 2,
                    HealthPower = 2,
                    RagePower = 2,
                    RageIncrement = 0.5f,
                    ActionGaugeIncrement = 0.2f,
                    HealthPoints = 10f,
                    ActionGaugePoints = 10f,
                    RagePoints = 0,
                    currentItemInUse = new ConsumableModel {
                        id = "0000",
                        quantity = defaultItemCount
                    },
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
                if (AccountManager.Instance.AccountData.HasCurrentActiveGame == false)
                {
                    AccountManager.Instance.ResetPlayerStatus();
                }
                AccountManager.Instance.SetActiveGame(true, true);
                playerData = AccountManager.Instance.AccountData.CurrentCharacterData;
                int maxItem = playerData.currentItemInUse.effectType == ItemEffectType.HEALING ? 10 : 5;
                int itemCount = playerData.currentItemInUse.quantity;
                playerData.MaxItemCount = maxItem;
                playerData.ItemCount = itemCount > maxItem ? maxItem : itemCount;
            }
            Manager.AccountDataInit(playerData);
        }
    }
}