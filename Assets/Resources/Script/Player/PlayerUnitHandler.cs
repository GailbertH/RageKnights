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
        [SerializeField] private PlayerUnitController units;

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
                return units?.GetUnitData?.HealthPoints > 0;
            }
        }

        public PlayerUnitModel GetPlayerData
        {
            get
            {
                return units?.GetUnitData;
            }
        }

        public void PlayerInitialize(PlayerUnitModel playerData)
        {
            units?.Init(playerData);
            Debug.Log("Player Initialize");
        }

        public EnemyHandler GetEnemyHandler()
        {
            return GameManager.Instance.EnemyHandler;
        }

        ///////////////////////////////////////////////////// 
        private bool playerTurnIsDone = false;
        public void SetTurnOrder()
        {
        }

        public string CurrentUnitAtTurn()
        {
            return "";
        }

        public bool IsTurnsFinished()
        {
            //All units done doing their turn
            return playerTurnIsDone;
        }

        public void UpdateTurns()
        {
            //Temp
            playerTurnIsDone = true;
        }

        public void ResetTurns()
        {
            //All unit turnIsDone = false;
            playerTurnIsDone = false;
        }

        /////////////////////////////////////////////////////

        public void UpdateHealthGauge()
        {
            float playerHP = GetPlayerData != null ? GetPlayerData.HealthPoints : 0f;
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerHealth(playerHP);
        }

        public void PlayerDamaged(int damage)
        {
            UpdateHealthGauge();
        }

        public void PlayerAttack()
        {
            currentPlayerState = PlayerState.ATTACKING;
            if (units?.IsAttackPlaying() == false)
            {
                units?.PlayAttackAnimation();
                float attackDamage = GetPlayerData != null ? units.GetUnitData.AttackPower : 0;
                GetEnemyHandler().DamagedEnemy(attackDamage);
            }
        }

        public void PlayerResetAnimation()
        {
            units?.ResetAnimation();
            currentPlayerState = PlayerState.IDLE;
            Debug.Log("Player IdleS");
        }

        public void PlayerMoveForward()
        {
            currentPlayerState = PlayerState.IDLE;
            units.PlayMoveAnimation();
            //Debug.Log("Player Forward");
        }

    }
}
