using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YesAndEngine.GameStateManagement;

namespace SubwaySpraypainter {

	// Main menu game state.
	public class MainMenuGameState : IGameState {

		// Called when the play button is clicked.
		public void OnClickPlay() {
			Manager.PushState("Gameplay");
		}
	}
}