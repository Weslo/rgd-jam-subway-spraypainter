using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages tweens.
	public class TweenManager : SingletonMonobehavior<TweenManager> {

		// An instance of a tween.
		private class TweenInstance {

			// Duration of this tween.
			internal float duration;

			// Current time of this tween.
			internal float time;

			// Step action.
			internal Action<float> step;

			// On complete action.
			internal Action onComplete;

			// If set to true, this tween is completed.
			internal bool completed = false;

			// Constructor.
			internal TweenInstance(float duration, Action<float> step, Action onComplete = null) {
				this.duration = duration;
				this.step = step;
				this.onComplete = onComplete;
				time = 0;
			}

			// Update this instance.
			internal void Update(float dt) {
				if(completed) {
					return;
				}
				time += dt;
				if(time >= duration) {
					time = duration;
					completed = true;
				}
				if(step != null) {
					step(GetInterpolation());
				}
				if(completed) {
					if(onComplete != null) {
						onComplete();
					}
				}
			}

			// Calculate interpolation value.
			private float GetInterpolation() {
				return time / duration;
			}
		}

		// List of active tweens.
		private List<TweenInstance> tweens = new List<TweenInstance>();

		// Update this component between frames.
		void Update() {
			List<TweenInstance> toRemove = new List<TweenInstance>();
			foreach(TweenInstance tween in tweens) {
				tween.Update(Time.deltaTime);
				if(tween.completed) {
					toRemove.Add(tween);
				}
			}
			foreach(TweenInstance tween in toRemove) {
				tweens.Remove(tween);
			}
		}

		// Add a new tween.
		public static void Tween(float duration, Action<float> step, Action onComplete = null) {
			Instance.tweens.Add(new TweenInstance(duration, step, onComplete));
		}
	}
}