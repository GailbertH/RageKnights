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
        private static GameManager instance = null;
		private RageKnightStateMachine stateMachine = null;
        private Coroutine updateRoutine = null;
        private Coroutine timeTrackerRoutine = null;

        private bool isStateActive = false;
        private bool isGamePaused = false;
        private int stageTracker = 0;

        [SerializeField] private EnemyHandler enemyHandler;
        [SerializeField] private PlayerHandler playerHandler;
        [SerializeField] private EnvironmentHandler environmentHandler;
        [SerializeField] private int stageCount;

        #region Properties
        public static GameManager Instance { get { return instance; } }

        private GameUIManager gameUIManager; 
        public GameUIManager GameUIManager
        {
            get { return gameUIManager; }
            set { gameUIManager = value; }
        }

        public PlayerHandler PlayerHandler
        {
            get { return playerHandler; }
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

        public int StageCount
        {
            get { return stageCount; }
        }

        public int StageTracker
        {
            get { return stageTracker; }
            set { stageTracker = value; }
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
            playerHandler.PlayerInitialize();
            updateRoutine = StartCoroutine(ControlledUpdate());
            timeTrackerRoutine = StartCoroutine(TimeTracker());
        }

		void OnDestroy()
		{
            isStateActive = false;
            if (stateMachine != null)
			{
                stateMachine.Destroy ();
				stateMachine = null;
			}
            if (updateRoutine != null)
            {
                StopCoroutine(updateRoutine);
            }
        }

		private IEnumerator ControlledUpdate()
		{
            //yield return new WaitForEndOfFrame();
            while (isStateActive) 
			{
                yield return new WaitForEndOfFrame();
                if (isGamePaused == false)
                {
                    stateMachine.Update();
                }
            }
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
        #endregion

        public void PauseGame(bool isPause)
        {
            isGamePaused = isPause;
        }
    }
}