using System;
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
            //Manager.StateMachine.SwitchState(RageKnightState.INGAME);
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
            string combatId = Guid.NewGuid().ToString();
            List<PlayerUnitModel> playerDataList = new List<PlayerUnitModel>();
            //if (Manager.isTestMode == true)
            //{
            string[] unitNames = { "Lancelot", "Vira", "Albert" };
            for (int i = 0; i < unitNames.Length; i++)
            {
                PlayerUnitModel playerData = new PlayerUnitModel
                {
                    name = unitNames[i],
                    unitCombatID = combatId,
                    healthPoints = 100,
                    manaPoints = 100,
                    ragePoints = 0,
                    rageIncrement = 1,
                    attackPower = 2,
                    defensePower = 2,
                    vitalityPower = 2
                };

                playerDataList.Add(playerData);
            }
            //}
            //if (playerData == null)
            //{
            //    if (AccountManager.Instance.AccountData.HasCurrentActiveGame == false)
            //    {
            //        AccountManager.Instance.ResetPlayerStatus();
            //    }
            //    AccountManager.Instance.SetActiveGame(true, true);
            //    playerData = AccountManager.Instance.AccountData.CurrentCharacterData;
            //}
            Manager.AccountDataInit(playerDataList);
        }
    }
}