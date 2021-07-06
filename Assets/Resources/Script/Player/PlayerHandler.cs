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

        [SerializeField] private GameObject soldierHolder;
        [SerializeField] private Animation soldierAnim;

        private PlayerState currentPlayerState = PlayerState.IDLE;
        private bool isInRageMode = false;
        private bool isItemOnCoolDown = false;
        private bool isActionGaugeFull = false;
        private float rageDuration = 10f;
        private float healCooldown = 5f;
        private const float BASE_RAGE_DURATION = 10f;
        private const float BASE_ITEM_COOLDOWN = 5f;

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
                return player?.GetPlayerData?.HealthPoints > 0;
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

        public void PlayerInitialize(PlayerModel playerData)
        {
            isInRageMode = false;
            player?.Init(playerData);
            Debug.Log("Player Initialize");
        }

        public void UpdateItemCount()
        {
            GameManager.Instance.GameUIManager.UpdateItemButtonText(player.ItemCount);
        }

        public void UpdateRageGauge()
        {
            player?.PlayerRagePoints(GetPlayerData.RagePower * GetPlayerData.RageIncrement);
            GameManager.Instance.GameUIManager.UpdateRageMeter(GetPlayerData.RagePoints, GetPlayerData.MaxRagePoints);
        }

        public void UpdateActionGuage()
        {
            if (IsActionGuageFull == true)
            {
                return;
            }

            PlayerModel playerData = GetPlayerData;
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
            player?.ReduceActionPoints(10);
            isActionGaugeFull = (bool)player?.isActionGaugeFull;
            float playerAGP = (float)playerData?.ActionGaugePoints;
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerActionGauge(playerAGP);
        }

        public void UpdateHealthGauge()
        {
            float playerHP = GetPlayerData != null ? GetPlayerData.HealthPoints : 0f;
            GameManager.Instance.GameUIManager.HealthbarHandler.UpdatePlayerHealth(playerHP);
        }

        public void PlayerRageActivate()
        {
            if (GetPlayerData.RagePoints >= GetPlayerData.MaxRagePoints)
            {
                isInRageMode = true;
                rageDuration = BASE_RAGE_DURATION;
                player?.PlayerUseRageMode();
                Debug.Log("Player Rage Mode");
            }
        }

        public void PlayerRageDeactivate()
        {
            isInRageMode = false;
            player?.RevertBackToNormal();
            Debug.Log("Player Normal Mode");
        }

        private void PlayerItemCoolDown(bool isOnCoolDown)
        {
            isItemOnCoolDown = isOnCoolDown;
            healCooldown = BASE_ITEM_COOLDOWN;
            GameManager.Instance.GameUIManager.ItemButtonStatus(!isOnCoolDown && GetPlayerData.ItemCount > 0);
        }

        public void PlayerAddItem(int itemCount)
        {
            player?.PlayerAddItem(itemCount);
            UpdateItemCount();
        }

        public void PlayerUseItem()
        {
            if (isItemOnCoolDown == false)
            {
                PlayerItemCoolDown(true);
                player?.PlayerHeal();
                UpdateHealthGauge();
                GameManager.Instance.GameUIManager.UpdateItemButtonText(player.ItemCount);
                Debug.Log("Player Use Item");
            }
        }

        public void PlayerDamaged(int damage)
        {
            player?.PlayerDamaged(damage);
            UpdateHealthGauge();
        }

        public void PlayerAttack(GameManager manager)
        {
            currentPlayerState = PlayerState.ATTACKING;
            if (player?.IsAttackPlaying() == false)
            {
                player?.PlayAttackAnimation();
                float attackDamage = GetPlayerData != null ? player.TotalAttackDamage : 0;
                manager?.EnemyHandler?.DamagedEnemy(attackDamage);
                UpdateRageGauge();
            }
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
            //Debug.Log("Player Forward");
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
            if (isInRageMode == true)
            {
                rageDuration--;
                if (rageDuration <= 0)
                {
                    PlayerRageDeactivate();
                }
            }
            if (isItemOnCoolDown == true)
            {
                healCooldown--;
                if (healCooldown <= 0)
                {
                    PlayerItemCoolDown(false);
                }
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

        //RAGE TEMP


        public void SetupRageMode()
        {
            soldierHolder.SetActive(true);
            soldierAnim.Play("Temp_Charge");
            player.GetComponent<Transform>().localPosition = new Vector3(0f, 1f, 1f);
        }

        public void EndRageMode()
        {
            player.GetComponent<Transform>().localPosition = new Vector3(-1.25f, 1f, 1f);
            soldierAnim.Play("Temp_Fade_In_Battle");
        }

        public void HideRageSoldier()
        {
            soldierHolder.SetActive(false);
        }
    }
}
