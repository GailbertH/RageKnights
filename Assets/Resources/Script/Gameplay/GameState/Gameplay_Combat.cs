using RageKnight;
using RageKnight.GameState;
using System;

public class Gameplay_Combat : GameplayState_Base<GameplayState>
{
    private const int NEXT_STATE_COUNT_DOWN = 2;
    private bool startCountDown = false;
    private int countDownTimer = NEXT_STATE_COUNT_DOWN;
    private Action onStateEndAction = null;
    GameUIManager Controls = null;

    public Gameplay_Combat(GameManager manager, RageKnight_InGame handler) : base(GameplayState.COMBAT, manager, handler)
    {
    }

    private GameplayState nextState = GameplayState.ADVENTURE;

    public override void GameGoToNextState()
    {
        Handler.SwitchState(nextState);
    }

    public override bool GameAllowTransition(GameplayState nextState)
    {
        return (nextState == GameplayState.ADVENTURE || 
            nextState == GameplayState.RESULT);
    }

    public override void GameStart()
    {
        Controls = Manager.GameUIManager;
        Controls.UpdateControlMode(State);
        Controls.UpdateMiddleUIModle(State);

        startCountDown = false;
        countDownTimer = NEXT_STATE_COUNT_DOWN;

        Manager.PlayerHandler.UpdateItemCount();
        Manager.PlayerHandler.PlayerResetAnimation();
    }

    public override void GameEnd()
    {
        if (Manager?.EnemyHandler?.IsAlive == false)
        {
            Manager.EnemyHandler.UnSetEnemy();
            Manager.PlayerHandler.ResetPassiveBenifits();
            Manager.IncrementStage(false);
        }
    }

    public override void GameUpdate()
    {
        if (startCountDown == false && Manager != null)
        {
            CheckPlayerAction();
            CheckEnemyAction();
        }
    }

    public override void GameTimerUpdate()
    {
        if (startCountDown == true)
        {
            UnityEngine.Debug.Log("Is countdown started? " + startCountDown);
            if (countDownTimer <= 0)
            {
                countDownTimer = NEXT_STATE_COUNT_DOWN;
                GameGoToNextState();
            }
            else
            {
                countDownTimer--;
            }
        }

        else if (startCountDown == false && Manager != null)
        {
            CheckPlayerCooldown();
        }
    }

    private void CheckPlayerAction()
    {
        if (Controls != null)
        {
            if (Manager.PlayerHandler?.IsAlive == false)
            {
                nextState = GameplayState.RESULT;
                startCountDown = true;
                return;
            }

            Manager.PlayerHandler?.UpdateActionGuage();
            bool canAttack = (bool)Manager.PlayerHandler?.IsActionGuageFull;

            if (Controls.buttonEvents == BasicMovements.Attack && canAttack)
            {
                Manager.PlayerHandler?.PlayerAttack(Manager);
                Manager.PlayerHandler?.CheckHitTiming();
                Manager.PlayerHandler?.UseActionGauge();
            }
            else if (Controls.buttonEvents == BasicMovements.Idle)
            {
                Manager.PlayerHandler?.PlayerResetAnimation();
            }

            if (Controls.skillEvents == SkillMovements.Rage)
            {
                Manager.PlayerHandler?.PlayerRageActivate();
            }
            else if (Controls.skillEvents == SkillMovements.Heal)
            {
                Manager.PlayerHandler?.PlayerUseItem();
            }

            Controls.buttonEvents = BasicMovements.None;
            Controls.skillEvents = SkillMovements.None;

            if (Manager.EnemyHandler.IsAlive == false)
            {
                UnityEngine.Debug.Log("Is enemy Alive? " + Manager.EnemyHandler.IsAlive);
                //TODO Improve this someday
                long goldEarned = 10;
                Manager.UpdateGold(goldEarned);
                nextState = GameplayState.ADVENTURE;
                startCountDown = true;
            }
        }
    }
    
    private void CheckEnemyAction()
    {
        if(Manager.EnemyHandler != null)
        {
            Manager.EnemyHandler.EnemyActionChecker(Manager);
        }
    }

    private void CheckPlayerCooldown()
    {
        if(Manager.PlayerHandler != null)
        {
            Manager.PlayerHandler.CheckRageModeDuration();
        }
    }
}
