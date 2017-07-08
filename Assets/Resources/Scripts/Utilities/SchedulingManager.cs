using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages timers.
	public abstract class SchedulingManager<T> : SingletonMonobehavior<T> where T : SchedulingManager<T> {

		// An instance of a timer.
		public class TimerInstance {

			// If set to true, this timer is completed.
			public bool Completed {
				get;
				private set;
			}

			// Duration of this timer.
			private float duration;

			// Current time of this tween.
			private float time;

			// Step action.
			private Action<float> step;

			// On complete action.
			private Action onComplete;

			// Constructor.
			public TimerInstance(float duration, float delay = 0) {
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
			public TimerInstance OnStep(Action<float> step) {
				this.step = step;
				return this;
			}

			// Assign complete action.
			public TimerInstance OnComplete(Action onComplete) {
				this.onComplete = onComplete;
				return this;
			}

			// Calculate interpolation value.
			private float GetInterpolation() {
				return Mathf.Clamp(time, 0, duration) / duration;
			}
		}

		// List of active tweens.
		private List<TimerInstance> timers = new List<TimerInstance>();

		// Update this component between frames.
		void Update() {
			List<TimerInstance> toRemove = new List<TimerInstance>();
			foreach(TimerInstance timer in timers) {
				timer.Update(Time.deltaTime);
				if(timer.Completed) {
					toRemove.Add(timer);
				}
			}
			foreach(TimerInstance timer in toRemove) {
				timers.Remove(timer);
			}
		}

		// Add a new tween.
		protected static TimerInstance Begin(float duration, float delay = 0) {
			TimerInstance timer = new TimerInstance(duration, delay);
			Instance.timers.Add(timer);
			return timer;
		}
	}
}