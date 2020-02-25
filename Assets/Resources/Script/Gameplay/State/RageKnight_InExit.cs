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
			//END EVERYTHING
		}
	}
}