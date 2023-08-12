using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace RageKnight.Player
{
    public class PlayerUnitHandler : MonoBehaviour
    {
        [SerializeField] private List<PlayerUnitController> playerUnits;
        private PlayerUnitController currentActiveUnit = null;
        public bool IsAlive
        {
            get
            {
                return playerUnits.All(x => x.GetIsDead) == false;
            }
        }

        public List<UnitModel> GetPlayerData
        {
            get
            {
                return playerUnits.Select(x => x.UnitData).ToList();
            }
        }

        public Transform GetUnitTransform(string unitCombatId)
        {
            var pUnit = playerUnits.FirstOrDefault(x => x.GetUnitCombatId == unitCombatId);
            if (pUnit != null)
            {
                return pUnit.gameObject.transform;
            }
            else
                return null;
        }

        public void PlayerInitialize(List<UnitModel> playerDataList)
        {
            //need to adjust with 1 - 3 units
            for (int i = 0; i < playerUnits.Count; i++)
            {
                playerUnits[i].Initialize(playerDataList[i]);
                GameTargetingManager.Instance.AddTargetUnitToTheList(playerUnits[i].GetUnitCombatId, UnitSide.PLAYER);
            }

            currentActiveUnit = playerUnits[0];
        }

        public EnemyUnitHandler GetEnemyHandler()
        {
            return GameManager.Instance.EnemyHandler;
        }

        ///////////////////////////////////////////////////// 
        public string GetCurrentActiveUnitCombatId
        {
            get 
            {   
                if (currentActiveUnit == null)
                {
                    Debug.LogWarning("GGG - Inappropriate access");
                    return String.Empty;
                }
                return currentActiveUnit.GetUnitCombatId; 
            }
        }

        private bool playerTurnIsDone = false;
        public void SetTurnOrder(bool isPlayerGoesFirst = true)
        {
            playerTurnIsDone = isPlayerGoesFirst ? false : true;
        }

        public string CurrentUnitAtTurn()
        {
            return "";
        }

        public bool IsTurnsDone()
        {
            //All units done doing their turn
            return playerTurnIsDone;
        }

        public void TurnEnd()
        {
            currentActiveUnit.TurnEnd();
        }

        public void UpdateTurns()
        {
            bool isTurnsDone = true;
            currentActiveUnit = playerUnits.FirstOrDefault
                (x => x.GetIsTurnDone == false && x.GetIsDead == false);
            if (currentActiveUnit != null)
            {
                isTurnsDone = false;
            }
            playerTurnIsDone = isTurnsDone;
        }

        public void ResetTurns()
        {
            for (int i = 0; i < playerUnits.Count; i++)
            {
                playerUnits[i].ResetTurn();
            }
            currentActiveUnit = playerUnits[0];
            playerTurnIsDone = false;
        }
        /////////////////////////////////////////////////////

        public void DamagedPlayer(int damage, List<string> targetCombatIDs)
        {
            if (targetCombatIDs.Count <= 0)
                return;

            foreach (string targetCombatID in targetCombatIDs)
            {
                DamagePlayerUnit(targetCombatID, damage);
            }
        }

        public void DamagePlayerUnit(string targetCombatID, int damageAmount)
        {

            var playerUnit = playerUnits.Find(x => x.GetUnitCombatId == targetCombatID);

            if (playerUnit != null)
            {
                var currentHP = playerUnit.DamageHealth(damageAmount);
                GameUIManager.Instance.HealthbarHandler.UpdateHealthPoints(targetCombatID, currentHP);
                if (playerUnit.GetIsDead)
                {
                    GameTargetingManager.Instance.RemoveTargetUnitToTheList(playerUnit.GetUnitCombatId, UnitSide.PLAYER);
                }
            }
        }

        public void PlayerAttack(Action onAnimationEnd = null)
        {
            currentActiveUnit.Attack(onAnimationEnd);
        }

        public void PlayerResetAnimation()
        {
            currentActiveUnit?.ResetAnimation();
            Debug.Log("Player IdleS");
        }

        public void RunTowards(Transform targetPosition, float speed, Action callback = null)
        {
            if (targetPosition == null)
            {
                Debug.Log("Run Towards no target");
                callback?.Invoke();
                return;
            }

            currentActiveUnit.RunTowards(targetPosition, speed, callback);
        }

        public void RunBackToSpot(float speed, Action callback = null)
        {
            currentActiveUnit.GoBackToInitialPosition(speed, callback);
        }
    }
}
