using System.Collections;
using System.Collections.Generic;
using RageKnight.Environment;
using RageKnight.GameState;
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
		private RageKnightStateMachine stateMachine = null;
        private CombatTracker combatTracker = null;

        private Coroutine timeTrackerRoutine = null;

        private bool isStateActive = false;
        private bool isGamePaused = false;
        private int stageTracker = 0;
        public int stage = 1;

        [SerializeField] private EnemyHandler enemyHandler;
        [SerializeField] private PlayerUnitHandler playerUnitHandler;
        [SerializeField] private EnvironmentHandler environmentHandler;
        [SerializeField] private int stageCount;

        /// <summary>
        /// Editor mode only.
        /// </summary>
        [SerializeField] public bool isTestMode = false;

        #region Properties
        public static GameManager Instance { get { return instance; } }

        private GameUIManager gameUIManager; 
        public GameUIManager GameUIManager
        {
            get { return gameUIManager; }
            set { gameUIManager = value; }
        }

        public PlayerUnitHandler PlayerHandler
        {
            get { return playerUnitHandler; }
        }

        public EnemyHandler EnemyHandler
        {
            get { return enemyHandler; }
        }

        public EnvironmentHandler EnvironmentHandler
        {
            get { return environmentHandler; }
        }

        public RageKnightStateMachine StateMachine
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

        #region Monobehavior Function
		void Awake()
		{
			instance = this;
		}

		void Start()
		{
            stateMachine = new RageKnightStateMachine(this);
            isStateActive = true;
            timeTrackerRoutine = StartCoroutine(TimeTracker());
            combatTracker = new CombatTracker("Stage"); //TODO Add functionality

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        #endregion

        private void Update()
        {
            if(isStateActive && isGamePaused == false)
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

        public void AccountDataInit(PlayerUnitModel playerData)
        {
            //TODO improvement needed goes ahead of init of UI
            PlayerHandler.PlayerInitialize(playerData);
            AddGold(0);
        }

        public void AddGold(long goldToAdd)
        {
            stageGold = AccountManager.Instance.AddGold(goldToAdd);
            combatTracker?.UpdateGoldEarned(goldToAdd);
            //this.GameUIManager?.UpdateGold(StageGold);
        }

        public void EnemyKill()
        {
            combatTracker.UpdateKillCount();
        }

        public void PauseGame(bool isPause)
        {
            isGamePaused = isPause;
        }

        public void IncrementStage()
        {
            stageTracker = stageTracker + 1;
            combatTracker.UpdateStageCompleteCount();
            GameUIManager.ProgressbarHandler.UpdateStage(stageTracker);
            Debug.Log("Stage: " + stage + " | " + stageTracker);
            AccountManager.Instance.UpdateStageProgress(stage, StageTracker, true);
        }

        public void GameOverReset()
        {
            AccountManager.Instance.UpdateStageProgress(stage, 0, false);
        }

        public void ExitGame()
        {
            StateMachine.Exit();
        }

        public void ExitingGame()
        {
            isStateActive = false;
            GameUIManager = null;
            playerUnitHandler = null;
            enemyHandler = null;
            environmentHandler = null;
            instance = null;
            if (stateMachine != null)
            {
                stateMachine.Destroy();
                stateMachine = null;
            }
        }


        ////////// Game play related /////////////
        public bool GetIsPlayerTurn
        {
            get { return PlayerHandler.IsTurnsDone() == false; }
        }

        public bool GetIsEnemyTurn
        {
            get { return EnemyHandler.IsTurnsDone() == false; }
        }
    }
}