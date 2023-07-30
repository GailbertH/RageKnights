using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace RageKnight.Player
{
    public enum PlayerState
    {
        IDLE = 0,
        ATTACKING = 1,
        DEAD = 2
    }

    public class PlayerUnitHandler : MonoBehaviour
    {
        [SerializeField] private List<PlayerUnitController> playerUnits;
        private PlayerUnitController currentActiveUnit = null;

        private PlayerState currentPlayerState = PlayerState.IDLE;

        public PlayerState GetPlayerState
        {
            get
            {
                if (playerUnits == null)
                    return PlayerState.IDLE;
                else
                    return currentPlayerState;
            }
        }

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

        public void PlayerInitialize(List<UnitModel> playerDataList)
        {
            //need to adjust with 1 - 3 units
            for (int i = 0; i < playerUnits.Count; i++)
            {
                playerUnits[i].Initialize(playerDataList[i]);
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

        public void DamagePlayerUnit(string targetCombatID, int damageAmount)
        {

            var playerUnit = playerUnits.Find(x => x.GetUnitCombatId == targetCombatID);

            if (playerUnit != null)
            {
                var currentHP = playerUnit.DamageHealth(damageAmount);
                GameUIManager.Instance.HealthbarHandler.UpdateHealthPoints(targetCombatID, currentHP);
            }
        }

        public void PlayerAttack()
        {
            currentPlayerState = PlayerState.ATTACKING;
            currentActiveUnit.Attack();
            int attackDamage = 0;//GetPlayerData != null ? currentActiveUnit.UnitData.attackPower : 0;
            GetEnemyHandler().DamagedEnemy(attackDamage, GameTargetingManager.Instance.GetTargets);

        }

        public void PlayerResetAnimation()
        {
            currentActiveUnit?.ResetAnimation();
            currentPlayerState = PlayerState.IDLE;
            Debug.Log("Player IdleS");
        }

        public void PlayerMoveForward()
        {
            currentPlayerState = PlayerState.IDLE;
            currentActiveUnit.PlayMoveAnimation();
            //Debug.Log("Player Forward");
        }
    }
}
