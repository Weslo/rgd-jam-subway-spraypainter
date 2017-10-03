using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YesAndEngine.GameStateManagement;

namespace SubwaySpraypainter {

	// Game over game state.
	public class GameOverGameState : IGameState {

		// Initialize this game state.
		public override void OnInitializeState() {
			base.OnInitializeState();
			GlobalAudioController.TransitionState("Equal");
		}

		// Called when the play button is clicked.
		public void OnClickPlay() {
			Manager.SwitchState("Gameplay");
		}
	}
}