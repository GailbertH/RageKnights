using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.Player
{
    public enum PlayerState
    {
        IDLE = 0,
        ATTACKING = 1,
        DEAD = 2
    }

    public class PlayerHandler : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        private PlayerState currentPlayerState = PlayerState.IDLE;
        private bool isInRageMode = false;
        private bool isActionGaugeFull = false;
        private float rageDuration = 10f;
        private const float BASE_RAGE_DURATION = 10f;

        public PlayerState GetPlayerState
        {
            get
            {
                if (player == null)
                    return PlayerState.IDLE;
                else
                    return currentPlayerState;
            }
        }

        public bool IsAlive
        {
            get
            {
                return player?.GetPlayerData.HealthPoints > 0;
            }
        }

        public bool IsActionGuageFull
        {
            get
            {
                return isActionGaugeFull;
            }
        }

        public PlayerModel GetPlayerData
        {
            get
            {
                return player?.GetPlayerData;
            }
        }

        public void PlayerInitialize()
        {
            isInRageMode = false;
            player?.Init();
            Debug.Log("Player Initialize");
        }

        public void UpdateRageGauge(float value)
        {
        }

        public void UpdateActionGuage()
        {
            if (IsActionGuageFull == true)
            {
                return;
            }

            PlayerModel playerData = player?.GetPlayerData;
            float value = (float)playerData?.ActionGaugeIncrement;
            player?.PlayerActionGauge(value);
            isActionGaugeFull = (bool)player?.isActionGaugeFull;
            float playerAGP = (float)playerData?.ActionGaugePoints;

            GameUIManager gameUiManager = GameManager.Instance.GameUIManager;
            gameUiManager.HealthbarHandler.UpdatePlayerActionGauge(playerAGP);
            if (isActionGaugeFull == true)
            {
                gameUiManager.PlayTimingNotif();
            }
        }
        public void UpdateActionGuage(float value)
        {
            if (IsActionGuageFull == true)
            {
                return;
            }

            PlayerModel playerData = player?.GetPlayerData;
            player?.PlayerActionGauge(value);
            isActionGaugeFull = (bool)player?.isActionGaugeFull;
            float playerAGP = (float)playerData?.ActionGaugePoints;
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerActionGauge(playerAGP);
        }

        public void UseActionGauge()
        {
            PlayerModel playerData = player?.GetPlayerData;
            player?.ResetActionGauge();
            isActionGaugeFull = (bool)player?.isActionGaugeFull;
            float playerAGP = (float)playerData?.ActionGaugePoints;
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerActionGauge(playerAGP);
        }

        public void UpdateHealthGauge()
        {
            float playerHP = player?.GetPlayerData != null ? player.GetPlayerData.HealthPoints : 0f;
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerHealth(playerHP);
            Debug.Log("Heal Current Life " + playerHP);
        }

        public void PlayerRageActivate()
        {
            isInRageMode = true;
            rageDuration = BASE_RAGE_DURATION;
            player?.RageModifier();
            Debug.Log("Player Rage Mode");
        }

        public void PlayerRageDeactivate()
        {
            isInRageMode = false;
            player?.RevertBackToNormal();
            Debug.Log("Player Normal Mode");
        }

        public void PlayerUseItem()
        {
            player?.PlayerHeal();
            UpdateHealthGauge();
            Debug.Log("Player Use Item");
        }

        public void PlayerDamaged(int damage)
        {
            player?.PlayerDamaged(damage);
            UpdateHealthGauge();
        }

        public void PlayerAttack(GameManager manager)
        {
            currentPlayerState = PlayerState.ATTACKING;
            player?.PlayAttackAnimation();
            int attackDamage = player?.GetPlayerData != null ? player.GetPlayerData.AttackPower : 0;
            manager?.EnemyHandler?.DamagedEnemy(attackDamage);
        }

        public void PlayerResetAnimation()
        {
            player?.ResetAnimation();
            currentPlayerState = PlayerState.IDLE;
            Debug.Log("Player IdleS");
        }

        public void PlayerMoveForward()
        {
            currentPlayerState = PlayerState.IDLE;
            player.PlayMoveAnimation();
        }

        public void CheckHitTiming()
        {
            bool isHitTiming = GameManager.Instance.GameUIManager.IsTimingPlaying();
            if (isHitTiming == true)
            {
                ApplyPassiveBenifits();
                GameManager.Instance.GameUIManager.PlayTimingHit();
            }
            else
            {
                ResetPassiveBenifits();
            }
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerMaxActionGauge(player.CurrentMaxAGPoints);
        }

        public void CheckRageModeDuration()
        {
            if (isInRageMode == false)
                return;

            rageDuration--;
            if (rageDuration <= 0)
            {
                PlayerRageDeactivate();
            }
        }

        public void ApplyPassiveBenifits()
        {
            player.ActionGagugeModifier += player.CurrentMaxAGPoints > 25 ? 25 : 0;
        }

        public void ResetPassiveBenifits()
        {
            player.ActionGagugeModifier = 0;
        }
    }
}
