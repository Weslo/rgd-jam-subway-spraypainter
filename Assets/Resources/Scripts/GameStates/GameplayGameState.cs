using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YesAndEngine.GameStateManagement;

namespace SubwaySpraypainter {

	// Main menu game state.
	public class GameplayGameState : IGameState {

		// Spacing between wall segments.
		private const float WALL_SEGMENT_SPACING = 20;

		// Number of graffiti to spawn.
		private const int NUM_GRAFFITI = 3;

		// Reference to the spray paint particles object.
		[SerializeField]
		private ParticleSystem sprayPaintParticles;

		// VO to play when the player is busted.
		[SerializeField]
		private AudioClip bustedVO;

		// Sting to play when the player is busted.
		[SerializeField]
		private AudioClip bustedSting;

		// The current wall segment.
		private WallSegment current = null;

		// Painting state last frame.
		private bool prevPainting = false;

		// Initialize this game state.
		public override void OnInitializeState() {
			base.OnInitializeState();
			Graffiti.PaintingEnabled = true;
			sprayPaintParticles.GetComponent<Renderer>().sortingLayerName = "Foreground";
			GoToNextWall();
		}

		// Update this game state.
		public override void OnUpdateState() {
			base.OnUpdateState();

			if(Graffiti.Painting && !prevPainting) {
				sprayPaintParticles.Play();
			}

			if(Graffiti.Painting) {
				Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				sprayPaintParticles.transform.position = new Vector3(mp.x, mp.y, 0);
			}
			else {
				sprayPaintParticles.Stop();
			}
			prevPainting = Graffiti.Painting;
		}

		// Spawn the next wall segment.
		private void GoToNextWall() {

			// Cancel gameplay tweens.
			CancelGameplayTimers();

			// If this is the first wall, we're going to skip the transition animation.
			bool firstWall = current == null;

			// Get next wall x pos.
			float prevX = firstWall ? 0 : current.transform.position.x;
			float x = firstWall ? 0 : prevX + WALL_SEGMENT_SPACING;

			// Tween the camera to the next wall.
			WallSegment previous = current;

			// Spawn next wall.
			current = Instantiate(Resources.Load<WallSegment>("Prefabs/Wall Segment"), new Vector3(x, 0, 0), Quaternion.identity);
			current.transform.SetParent(transform, true);
			current.OnAllCompleted += OnWallComplete;
			current.OnPlayerCaught += OnPlayerCaught;

			// Transition to next wall.
			if(!firstWall) {
				TweenManager.Tween(1, 1).OnStep((t) => {
					Camera.main.transform.position = new Vector3(
						Mathf.Lerp(prevX, x, t),
						Camera.main.transform.position.y,
						Camera.main.transform.position.z
					);
				}).OnComplete(() => {
					if(previous != null) {
						current.SpawnGraffiti(NUM_GRAFFITI);
						Destroy(previous.gameObject);
					}
				});
			}
			else {
				current.SpawnGraffiti(NUM_GRAFFITI);
			}
		}

		// Called when a wall is completed.
		private void OnWallComplete(WallSegment wall) {
			GoToNextWall();
		}

		// Called when the player is caught.
		private void OnPlayerCaught(WallSegment wall) {
			Graffiti.PaintingEnabled = false;
			CancelGameplayTimers();
			GlobalAudioController.TransitionState("Sting");
			GlobalAudioController.Play("VO", bustedVO);
			GlobalAudioController.Play("Stings", bustedSting);
			TimerManager.Schedule(5).OnComplete(() => {
				Manager.SwitchState("Game Over");
			});
		}

		// Cancel all gameplay timers.
		private void CancelGameplayTimers() {
			TimerManager.Cancel("police");
			TweenManager.Cancel("police");
		}
	}
}