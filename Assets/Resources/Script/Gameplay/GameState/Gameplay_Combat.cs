using RageKnight;
using RageKnight.GameState;
using System;

public class Gameplay_Combat : GameplayState_Base<GameplayState>
{
    private float WALK_SPEED = -0.1f;
    private bool startCountDown = false;
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
            nextState == GameplayState.RAGE ||
            nextState == GameplayState.RESULT);
    }

    public override void GameStart()
    {
        base.GameStart();
        Controls = Manager.GameUIManager;
        startCountDown = false;

        Manager.PlayerHandler.UpdateItemCount();
        Manager.PlayerHandler.PlayerResetAnimation();

        Controls.UpdateControlMode(State);
        if (handler.GetPreviousState == GameplayState.ADVENTURE)
        {
            Controls.UpdateMiddleUIModle(State);
        }
    }

    public override void GameEnd()
    {
        if (Manager?.EnemyHandler?.HasSoldiers == false)
        {
            Manager.EnemyHandler.UnSetAllEnemy();
            Manager.PlayerHandler.ResetPassiveBenifits();
        }
    }

    public override void GameUpdate()
    {
        //UnityEngine.Debug.Log(Manager.PlayerHandler.GetPlayerState);
        if (startCountDown == false && Manager != null)
        {
            CheckPlayerAction();
        }
    }

    public override void GameTimerUpdate()
    {
        if (startCountDown == true)
        {
            UnityEngine.Debug.Log("Is countdown started? " + startCountDown);
            Controls.buttonEvents = BasicMovements.None;
            GameGoToNextState();
        }
        else if (Manager != null)
        {
            CheckPlayerCooldown();
            CheckCompanionAction();
            CheckEnemyAction();
        }
    }

    private void CheckPlayerAction()
    {
        if (Controls != null)
        {
            if (Manager.PlayerHandler?.IsAlive == false)
            {
                Manager.GameOverReset();
                nextState = GameplayState.RESULT;
                startCountDown = true;
                return;
            }

            Manager.PlayerHandler?.UpdateActionGuage();
            bool canAttack = true; //Manager.PlayerHandler?.GetPlayerData.ActionGaugePoints >= 10;

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
                UnityEngine.Debug.Log("Switching to Rage Mode");
                nextState = GameplayState.RAGE;
                startCountDown = true;
            }
            else if (Controls.skillEvents == SkillMovements.Heal)
            {
                Manager.PlayerHandler?.PlayerUseItem();
            }

            Controls.buttonEvents = BasicMovements.None;
            Controls.skillEvents = SkillMovements.None;

            if (Manager.EnemyHandler.HasSoldiers == false)
            {
                UnityEngine.Debug.Log("Is enemy Alive? " + Manager.EnemyHandler.HasSoldiers);
                //TODO Improve this someday
                Manager.IncrementStage();
                nextState = Manager.IsFinalStage ? GameplayState.RESULT : GameplayState.ADVENTURE;
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

    private void CheckCompanionAction()
    {
        if (Manager.PlayerHandler != null)
        {
            Manager.PlayerHandler.CheckCompanionAction(Manager);
        }
    }

    private void MoveEnvironment(float speed)
    {
        if (Manager.EnvironmentHandler != null)
        {
            Manager.EnvironmentHandler.MoveEnvironment(speed);
        }
    }
}

public enum CombatState
{
    WAITING,
    THINKNG,
    ACTION,
    RAGE,
    END
}
public class Combat_Base
{

}
//public class Combat_Waiting : Combat_Base<CombatState>
//{
//}
