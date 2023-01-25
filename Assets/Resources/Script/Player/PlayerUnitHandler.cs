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
                return units.All(x => x.UnitData.HealthPoints > 0);
            }
        }

        public PlayerUnitModel GetPlayerData
        {
            get
            {
                return units[0].UnitData;
            }
        }

        public void PlayerInitialize(PlayerUnitModel playerData)
        {
            units[0].UnitData = playerData;
            units[1].UnitData = playerData;
            units[2].UnitData = playerData;
            currentActiveUnit = units[0];
            Debug.Log("Player Initialize");
        }

        public EnemyHandler GetEnemyHandler()
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
            float playerHP = GetPlayerData != null ? GetPlayerData.HealthPoints : 0f;
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
            float attackDamage = GetPlayerData != null ? currentActiveUnit.UnitData.AttackPower : 0;
            GetEnemyHandler().DamagedEnemy(attackDamage);

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
