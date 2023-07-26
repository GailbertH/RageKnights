using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        [SerializeField] private List<PlayerUnitController> units;

        private PlayerState currentPlayerState = PlayerState.IDLE;
        private const float BASE_RAGE_DURATION = 10f;
        private const float BASE_ITEM_COOLDOWN = 5f;
        private int combatIdCounter = 0;

        public PlayerState GetPlayerState
        {
            get
            {
                if (units == null)
                    return PlayerState.IDLE;
                else
                    return currentPlayerState;
            }
        }

        public bool IsAlive
        {
            get
            {
                return units.All(x => x.UnitData.healthPoints > 0);
            }
        }

        public List<PlayerUnitModel> GetPlayerData
        {
            get
            {
                return units.Select(x => x.UnitData).ToList();
            }
        }

        public void PlayerInitialize(List<PlayerUnitModel> playerDataList)
        {
            for (int i = 0; i < playerDataList.Count; i++)
            {
                units[i].UnitData = playerDataList[i];
                units[i].Initialize(playerDataList[i].unitCombatID);
            }

            currentActiveUnit = units[0];
            Debug.Log("Player Initialize");
        }

        public EnemyUnitHandler GetEnemyHandler()
        {
            return GameManager.Instance.EnemyHandler;
        }

        ///////////////////////////////////////////////////// 
        PlayerUnitController currentActiveUnit = null;
        public PlayerUnitController GetCurrentActiveUnit
        {
            get { return currentActiveUnit; }
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
            //Temp
            bool isTurnsDone = true;
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].GetIsTurnDone == false)
                {
                    Debug.Log("Unit " + (i + 1) + " = " + units[i].GetIsTurnDone);
                    currentActiveUnit = units[i];
                    isTurnsDone = false;
                }
            }
            playerTurnIsDone = isTurnsDone;
            Debug.Log("playerTurnIsDone " + playerTurnIsDone);
        }

        public void ResetTurns()
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].ResetTurn();
            }
            currentActiveUnit = units[0];
            playerTurnIsDone = false;
        }
        /////////////////////////////////////////////////////

        public void UpdateHealthGauge()
        {
            //float playerHP = GetPlayerData != null ? GetPlayerData.healthPoints : 0f;
            //GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerHealth(playerHP);
        }

        public void PlayerDamaged(int damage)
        {
            UpdateHealthGauge();
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
