using System.Collections;
using System.Collections.Generic;
using RageKnight.Player;
using UnityEngine;
namespace RageKnight
{
    public class GameManager : MonoBehaviour
    {
        public const float WALK_SPEED = -0.05f;
        public const float ENEMY_WALK_SPEED = -0.05f;
        public const float RUN_SPEED = -0.1f;

        private static GameManager instance = null;
        private GameplayStateMachine stateMachine = null;
        private CombatTracker combatTracker = null;

        private Coroutine timeTrackerRoutine = null;

        private bool isStateActive = false;
        private bool isGamePaused = false;
        private int stageTracker = 0;
        public int stage = 1;

        [SerializeField] private EnemyUnitHandler enemyHandler;
        [SerializeField] private PlayerUnitHandler playerUnitHandler;
        [SerializeField] private int stageCount;

      
        /// <summary>
        /// Editor mode only.
        /// </summary>
        [SerializeField] public bool isTestMode = false;

        #region Properties
        public static GameManager Instance { get { return instance; } }

        public PlayerUnitHandler PlayerHandler
        {
            get { return playerUnitHandler; }
        }

        public EnemyUnitHandler EnemyHandler
        {
            get { return enemyHandler; }
        }

        public GameplayStateMachine StateMachine
		{
			get { return this.stateMachine; }
		}

        public CombatTracker CombatTracker
        {
            get { return this.combatTracker; }
        }

        private long stageGold;
        public long StageGold
        {
            get { return this.stageGold; }
        }

        public int StageCount
        {
            get { return stageCount; }
        }

        public int StageTracker
        {
            get { return stageTracker; }
        }

        public bool IsFinalStage
        {
            get { return StageTracker == StageCount; }
        }
        #endregion

		void Awake()
		{
			instance = this;
		}

		void Start()
		{
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            
            if (isTestMode)
            {
                Initialized();
            }
        }

        public void Initialized()
        {
            stateMachine = new GameplayStateMachine();
            combatTracker = new CombatTracker("Stage"); //TODO Add functionality

            stateMachine.Initilize();
            isStateActive = true;
            timeTrackerRoutine = StartCoroutine(TimeTracker());
        }

        private void Update()
        {
            if (isStateActive && isGamePaused == false)
                stateMachine?.Update();
        }

        private IEnumerator TimeTracker()
        {
            //yield return new WaitForEndOfFrame();
            while (isStateActive)
            {
                yield return new WaitForSeconds(1);
                if (isGamePaused == false)
                {
                    stateMachine.TimerUpdate();
                }
            }
        }

        public void ExecuteRoutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }

        public void PlayerUnitsInit(List<UnitModel> playerUnitModels)
        {
            //TODO improvement this needs to goes ahead of init of UI
            PlayerHandler.PlayerInitialize(playerUnitModels);
        }
        public void EnemyUnitsInit(List<UnitModel> playerUnitModels)
        {
        }

        public void PauseGame(bool isPause)
        {
            isGamePaused = isPause;
        }

        public void GameOverReset()
        {
            AccountManager.Instance.UpdateStageProgress(stage, 0, false);
        }

        public void ReturnBackToAdventure()
        {
            Debug.Log("ReturnBackToAdventure");
            ExitGame();
            if (SceneTransitionManager.Instance != null && RecordKeeperManager.Instance != null)
            {
                SceneTransitionManager.Instance.StartTransition(TransitionKey.COMBAT_TO_ADVENTURE);
                RecordKeeperManager.Instance.EnemyDefeated();
            }
            else
            {
                Debug.LogError("Scene Transition Manager is missing");
            }
        }

        public void ReturnToTitleScreen()
        {
            Debug.Log("ReturnToTitleScreen");
            ExitGame();
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.StartTransition(TransitionKey.COMBAT_TO_LOBBY);
            }
            else
            {
                Debug.LogError("Scene Transition Manager is missing");
            }
        }

        public void ExitGame()
        {
            isStateActive = false;
            instance = null;
            if (stateMachine != null)
            {
                StateMachine.Exit();
                stateMachine = null;
            }
        }


        ////////// Game play related /////////////
        ///Need to improve this
        public bool GetIsPlayerTurn
        {
            get { return PlayerHandler.IsTurnsDone() == false; }
        }

        public bool GetIsEnemyTurn
        {
            get { return EnemyHandler.IsTurnsDone() == false; }
        }

        public string GetCurrentAtTurnUnitCombatId
        {
            get 
            {
                if (PlayerHandler.IsTurnsDone() == false)
                {
                    return PlayerHandler.GetCurrentActiveUnitCombatId;
                }
                else if (EnemyHandler.IsTurnsDone() == false)
                {
                    return enemyHandler.GetCurrentActiveUnitCombatId;
                }

                return "";
            }
        }
    }
}