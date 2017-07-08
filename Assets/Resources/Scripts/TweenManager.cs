using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages tweens.
	public class TweenManager : SingletonMonobehavior<TweenManager> {

		// An instance of a tween.
		public class TweenInstance {

			// If set to true, this tween is completed.
			public bool Completed {
				get;
				private set;
			}

			// Duration of this tween.
			private float duration;

			// Current time of this tween.
			private float time;

			// Step action.
			private Action<float> step;

			// On complete action.
			private Action onComplete;

			// Constructor.
			public TweenInstance(float duration, float delay = 0) {
				this.duration = duration;
				time = -delay;
			}

			// Update this instance.
			internal void Update(float dt) {
				if(Completed) {
					return;
				}
				time += dt;
				if(time >= duration) {
					time = duration;
					Completed = true;
				}
				if(step != null) {
					step(GetInterpolation());
				}
				if(Completed) {
					if(onComplete != null) {
						onComplete();
					}
				}
			}

			// Assign step action.
			public TweenInstance OnStep(Action<float> step) {
				this.step = step;
				return this;
			}

			// Assign complete action.
			public TweenInstance OnComplete(Action onComplete) {
				this.onComplete = onComplete;
				return this;
			}

			// Calculate interpolation value.
			private float GetInterpolation() {
				return Mathf.Clamp(time, 0, duration) / duration;
			}
		}

		// List of active tweens.
		private List<TweenInstance> tweens = new List<TweenInstance>();

		// Update this component between frames.
		void Update() {
			List<TweenInstance> toRemove = new List<TweenInstance>();
			foreach(TweenInstance tween in tweens) {
				tween.Update(Time.deltaTime);
				if(tween.Completed) {
					toRemove.Add(tween);
				}
			}
			foreach(TweenInstance tween in toRemove) {
				tweens.Remove(tween);
			}
		}

		// Add a new tween.
		public static TweenInstance Tween(float duration, float delay = 0) {
			TweenInstance tween = new TweenInstance(duration, delay);
			Instance.tweens.Add(tween);
			return tween;
		}
	}
}