using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubwaySpraypainter {

	// Manages timers.
	public abstract class SchedulingManager<T> : SingletonMonobehavior<T> where T : SchedulingManager<T> {

		// An instance of a timer.
		public class TimerInstance {

			// If set to true, this timer is begun.
			public bool Begun {
				get;
				private set;
			}

			// If set to true, this timer is completed.
			public bool Completed {
				get;
				private set;
			}

			// Duration of this timer.
			private float duration;

			// Current time of this timer.
			private float time;

			// The id of this timer.
			public string ID {
				get;
				private set;
			}

			// Step action.
			private Action<float> step;

			// On begin action.
			private Action onBegin;

			// On complete action.
			private Action onComplete;

			// Constructor.
			public TimerInstance(float duration, float delay = 0, string id = null) {
				this.duration = duration;
				time = -delay;
				ID = id;
			}

			// Update this instance.
			internal void Update(float dt) {
				if(Completed) {
					return;
				}
				time += dt;
				if(!Begun && time >= 0) {
					Begun = true;
					if(onBegin != null) {
						onBegin();
					}
				}
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

			// Assign begin action.
			public TimerInstance OnBegin(Action onBegin) {
				this.onBegin = onBegin;
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

		// List of active timers.
		private List<TimerInstance> timers = new List<TimerInstance>();

		// Update this component between frames.
		void Update() {
			TimerInstance[] toUpdate = new TimerInstance[timers.Count];
			timers.CopyTo(toUpdate);
			foreach(TimerInstance timer in toUpdate) {
				timer.Update(Time.deltaTime);
				if(timer.Completed) {
					timers.Remove(timer);
				}
			}
		}

		// Add a new timer.
		protected static TimerInstance Begin(float duration, float delay = 0, string id = null) {
			TimerInstance timer = new TimerInstance(duration, delay, id);
			Instance.timers.Add(timer);
			return timer;
		}

		// Cancel timers with the specified id.
		public static void Cancel(string id) {
			TimerInstance[] toUpdate = new TimerInstance[Instance.timers.Count];
			Instance.timers.CopyTo(toUpdate);
			foreach(TimerInstance timer in toUpdate) {
				if(!string.IsNullOrEmpty(timer.ID) && timer.ID.Equals(id)) {
					Instance.timers.Remove(timer);
				}
			}
		}
	}
}