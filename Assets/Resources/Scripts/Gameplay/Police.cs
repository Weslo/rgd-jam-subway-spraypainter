using System;
using UnityEngine;

namespace SubwaySpraypainter {

	// Police officer controller.
	public class Police : MonoBehaviour {

		// Random VO lines to play as warnings.
		[SerializeField]
		private AudioClip[] warningVO;

		// If set to true, this police officer is activated.
		private bool active = false;

		// Fires when this police catches the player.
		public Action<Police> OnCatchPlayer;

		// Update this component.
		void Update() {
			if(active && Graffiti.Painting) {
				CatchPlayer();
				active = false;
			}
		}

		// Activate this officer.
		public void Activate() {

			// Play random VO line.
			GlobalAudioController.Play("VO", warningVO.PickRandom());

			// Move up from spawn.
			Vector3 spawnPos = transform.position;
			Vector3 targetPos = transform.position + Vector3.up * 2;
			TweenManager.Tween(3, 0, "police").OnStep((t) => {
				transform.position = Vector3.Lerp(spawnPos, targetPos, t);
			}).OnComplete(() => {
				active = true;
				TweenManager.Tween(1, 3, "police").OnStep((t) => {
					transform.position = Vector3.Lerp(targetPos, spawnPos, t);
				}).OnBegin(() => {
					active = false;
				}).OnComplete(() => {
					Destroy(gameObject);
				});
			});
		}

		// Catch the player.
		private void CatchPlayer() {
			if(OnCatchPlayer != null) {
				OnCatchPlayer(this);
			}
			GetComponent<SpriteRenderer>().sortingLayerName = "Default";
			GetComponent<Animator>().Play("Caught");

			Vector2 prevPos = transform.position;
			TweenManager.Tween(0.25f, 0).OnStep((t) => {
				transform.position = Vector2.Lerp(prevPos, Camera.main.transform.position, t);
			});
		}
	}
}