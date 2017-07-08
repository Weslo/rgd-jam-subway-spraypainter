using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Police officer controller.
	public class Police : MonoBehaviour {

		// If set to true, this police officer is activated.
		private bool active = false;

		// Activate this officer.
		public void Activate() {
			Vector3 spawnPos = transform.position;
			Vector3 targetPos = transform.position + Vector3.up * 2;
			TweenManager.Tween(3, 0, "police").OnStep((t) => {
				transform.position = Vector3.Lerp(spawnPos, targetPos, t);
			}).OnComplete(() => {
				active = true;
				TweenManager.Tween(1, 3, "police").OnStep((t) => {
					transform.position = Vector3.Lerp(targetPos, spawnPos, t);
				}).OnComplete(() => {
					active = false;
					Destroy(gameObject);
				});
			});
		}
	}
}