using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.GameState
{
	public class RageKnight_Exit : RageKnightState_Base<RageKnightState>
	{
		public RageKnight_Exit (GameManager manager) : base (RageKnightState.EXIT, manager)
		{
		}
			
		public override void Start ()
		{
            //TODO IMPROVE THIS SHIT
            PlayerModel currentPlayerData = GameManager.Instance.PlayerHandler.GetPlayerData;
            PlayerModel newPlayerData = AccountManager.Instance.PlayerData;
            newPlayerData.ItemCount = currentPlayerData.ItemCount;

            AccountManager.Instance.PlayerData = newPlayerData;

            //START UNLOADING
            if (LoadingManager.Instance != null)
            {
                LoadingManager.Instance.SetSceneToUnload(SceneNames.GAME_UI + "," + SceneNames.GAME_SCENE);
                LoadingManager.Instance.SetSceneToLoad(SceneNames.LOBBY_SCENE);
                LoadingManager.Instance.LoadGameScene();
            }
		}
		public override void End () 
		{
            Manager.ExitingGame();
        }
	}
}